﻿using System;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using alpine.api.Filters;
using alpine.database.Models;
using alpine.repository;

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
            // Add framework services.
            services.AddCors( options => options.AddPolicy( "AllowAll", p => p.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader() ) );

            services.AddEntityFrameworkSqlServer()
                       .AddDbContext<alpineContext>( options => options.UseSqlServer( Configuration[ "Data:DefaultConnection:ConnectionString" ] ) );

            alpineContext.ConnectionString = Configuration[ "Data:DefaultConnection:ConnectionString" ];

            services.AddScoped( typeof( IRepository<> ), typeof( Repository<> ) );

            services.AddMvc( options =>
            {
                options.Filters.Add( new AlpineExceptionFilter() );
            } );
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

            ConfigureOAuth( app );

            app.UseMvc();
        }

        public void ConfigureOAuth( IApplicationBuilder app )
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

            app.UseJwtBearerAuthentication(
                new JwtBearerOptions
                {
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true,
                    TokenValidationParameters = tokenValidationParameters
                }
            );
        }
    }
}
