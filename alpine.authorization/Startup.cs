using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace alpine.authorization
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

		private static readonly string secretKey = "secretsecretsecretsecretkeyABC123!";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);

			app.UseSimpleTokenProvider(new TokenProviderOptions
			{
				Path = "/token",
				Audience = "ExampleAudience",
				Issuer = "ExampleIssuer",
				SigningCredentials = signingCredentials,
				IdentityResolver = GetIdentity
			});

            app.UseMvc();
        }

		Task<ClaimsIdentity> GetIdentity(string username, string password)
		{
			if (username == "TEST" && password == "TEST123")
			{
				return Task.FromResult(new ClaimsIdentity(new System.Security.Principal.GenericIdentity(username, "Token"), new Claim[] { }));
			}

			// Credentials are invalid, or account doesn't exist
			return Task.FromResult<ClaimsIdentity>(null);
		}
	}
}
