using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using alpine.core;

namespace alpine.api.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware( RequestDelegate next )
        {
            _next = next;
        }

        public Task Invoke( HttpContext context, ApiKeyAccessor apiKeyAccessor )
        {
            StringValues key;
            if ( context.Request.Headers.TryGetValue( "Authorization", out key ) )
            {
                apiKeyAccessor.ApiKey = key;
                return _next( context );
            }

            throw new AlpineException( "Couldn't retrieve token." );
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKey(
            this IApplicationBuilder builder )
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
