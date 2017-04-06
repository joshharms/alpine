using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace alpine.authorization
{
	public static class TokenProviderAppBuilderExtensions
	{
		public static IApplicationBuilder UseSimpleTokenProvider(this IApplicationBuilder app, TokenProviderOptions options)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			return app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));
		}
	}
}
