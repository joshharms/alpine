using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using alpine.database.Models;
using alpine.service;
using alpine.service.Interfaces;

namespace alpine.authorization
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
            services.AddEntityFrameworkSqlServer()
                 .AddDbContext<alpineContext>( options => options.UseSqlServer( Configuration[ "Data:DefaultConnection:ConnectionString" ] ) );

            alpineContext.ConnectionString = Configuration[ "Data:DefaultConnection:ConnectionString" ];

            services.AddScoped<IPasswordService, PasswordService>();

            services.AddMvc();
        }

        private static readonly string secretKey = "secretsecretsecretsecretkeyABC123!";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddConsole( Configuration.GetSection( "Logging" ) );
            loggerFactory.AddDebug();

            var signingCredentials = new SigningCredentials(
                 new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) ), SecurityAlgorithms.HmacSha256 );
            var signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "ExampleIssuer",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "ExampleAudience",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseSimpleTokenProvider( new TokenProviderOptions
            {
                Path = "/token",
                RefreshPath = "/refresh-token",
                Audience = "ExampleAudience",
                Issuer = "ExampleIssuer",
                SigningCredentials = signingCredentials,
                IdentityResolver = GetIdentity
            }, tokenValidationParameters );

            app.UseDeveloperExceptionPage();
            app.UseMvc();
        }

        Task<ClaimsIdentity> GetIdentity( string username, string password )
        {
            IPasswordService p = new PasswordService( new alpineContext() );

            if( p.ValidatePassword( username, password ) )
            {
                return Task.FromResult( new ClaimsIdentity( new System.Security.Principal.GenericIdentity( username, "Token" ), new Claim[] { } ) );
            }

            // Credentials are invalid, or account doesn't exist
            return Task.FromResult<ClaimsIdentity>( null );
        }
    }
}
