using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using alpine.core;

namespace alpine.api.Middleware
{
    public class AuthenticationTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationTokenMiddleware( RequestDelegate next )
        {
            _next = next;
        }

        public Task Invoke( HttpContext context, AuthenticationTokenAccessor authenticationTokenAccessor )
        {
            authenticationTokenAccessor.token = GetToken( context );

            return _next( context );

        }

        protected AuthenticationToken GetToken( HttpContext httpContext )
        {
            var claims = httpContext.User.Claims;

            AuthenticationToken token = new AuthenticationToken();

            if ( claims.Any( c => c.Type == "user_id" ) )
            {
                try
                {
                    token.userId = Guid.Parse( claims.Where( x => x.Type == "user_id" ).Select( x => x.Value ).Single() );
                    token.email = claims.Where( x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" ).Select( x => x.Value ).Single();
                    token.firstName = claims.Where( x => x.Type == "first_name" ).Select( x => x.Value ).Single();
                    token.lastName = claims.Where( x => x.Type == "last_name" ).Select( x => x.Value ).Single();
                    token.role = claims.Where( x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" ).Select( x => x.Value ).Single();
                    //token.organizationId = Guid.Parse( claims.Where( x => x.Type == "organization_id" ).Select( x => x.Value ).Single() );
                    token.avatar = claims.Where( x => x.Type == "avatar" ).Select( x => x.Value ).Single();
                }
                catch ( Exception ex )
                {
                    throw new AlpineException( ex.InnerException.Message );
                }
            }

            return token;
        }
    }

    public static class AuthenticationTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationToken(
            this IApplicationBuilder builder )
        {
            return builder.UseMiddleware<AuthenticationTokenMiddleware>();
        }
    }
}
