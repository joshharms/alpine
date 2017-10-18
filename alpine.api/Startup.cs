using System;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using alpine.api.Filters;
using alpine.api.Middleware;
using alpine.core;
using alpine.database.Models;
using alpine.repository;
using alpine.service.Interfaces;
using alpine.service.Services;

namespace alpine.api
{
    public class Startup
    {
        public Startup( IHostingEnvironment env )
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath( env.ContentRootPath )
                .AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true )
                .AddJsonFile( $"appsettings.{env.EnvironmentName}.json", optional: true )
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            ConfigureOAuth( services );

            // Add framework services.
            services.AddCors( options => options.AddPolicy( "AllowAll", p => p.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader() ) );

            services.AddEntityFrameworkSqlServer()
                       .AddDbContext<alpineContext>( options => options.UseSqlServer( Configuration[ "Data:DefaultConnection:ConnectionString" ] ) );

            alpineContext.ConnectionString = Configuration[ "Data:DefaultConnection:ConnectionString" ];

            services.AddScoped( typeof( IRepository<> ), typeof( Repository<> ) );
            services.AddScoped<IUserService, UserService>();

            services.AddMvc( options =>
            {
                options.Filters.Add( new AlpineExceptionFilter() );
            } );

            services.AddScoped( typeof( AuthenticationTokenAccessor ) );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddConsole( Configuration.GetSection( "Logging" ) );
            loggerFactory.AddDebug();

            app.UseCors( "AllowAll" );

            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            //Provides the token to be used by services, must come after "app.UseAuthentication();"
            app.UseAuthenticationToken();

            app.UseMvc();
        }

        public void ConfigureOAuth( IServiceCollection services )
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( Configuration[ "AppSettings:Auth:Secret" ] ) ),

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration[ "AppSettings:Auth:Issuer" ],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration[ "AppSettings:Auth:Audience" ],

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
                .AddJwtBearer( options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                } );
        }
    }
}
