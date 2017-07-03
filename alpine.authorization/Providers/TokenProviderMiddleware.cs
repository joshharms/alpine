using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using alpine.database.Models;
using alpine.service.Interfaces;

namespace alpine.authorization
{
    public class TokenProviderMiddleware : IDisposable
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly alpineContext _dbContext;
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenProviderMiddleware(
            RequestDelegate next, IOptions<TokenProviderOptions> options,
            TokenValidationParameters tokenValidationParameters, ILoggerFactory loggerFactory,
            alpineContext context, IRefreshTokenService refreshTokenService )
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<TokenProviderMiddleware>();
            _dbContext = context;
            _refreshTokenService = refreshTokenService;

            _tokenValidationParameters = tokenValidationParameters;

            _options = options.Value;
            if ( tokenValidationParameters == null )
            {
                throw new ArgumentNullException( nameof( tokenValidationParameters ) );
            }
            ThrowIfInvalidOptions( _options );

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Task Invoke( HttpContext context )
        {
            // first figure out whether this is a request for a new token or for a refresh
            bool isCreate = context.Request.Path.Equals( _options.Path, StringComparison.Ordinal );
            bool isRefresh = !isCreate && context.Request.Path.Equals( _options.RefreshPath, StringComparison.Ordinal );

            // If the request path doesn't match, skip
            if ( !isCreate && !isRefresh )
            {
                return _next( context );
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if ( !context.Request.Method.Equals( "POST" )
               || !context.Request.HasFormContentType )
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync( "Bad request." );
            }

            _logger.LogInformation( $"Handling request for {( isCreate ? "create token" : "refresh token" )}: " + context.Request.Path );

            if ( isCreate )
            {
                return NewToken( context );
            }
            else
            {
                return IssueRefreshedToken( context );
            }
        }

        private async Task NewToken( HttpContext context )
        {
            string username = context.Request.Form[ "username" ];
            var password = context.Request.Form[ "password" ];
            Guid audienceId = Guid.Parse( context.Request.Form[ "audience_id" ].First() );
            string clientId = context.Request.Form[ "client_id" ];

            var identity = await _options.IdentityResolver( username, password );
            if ( identity == null )
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync( "Invalid username or password." );
                return;
            }

            await GenerateToken( context, username, audienceId, clientId );
        }

        private async Task IssueRefreshedToken( HttpContext context )
        {
            try
            {
                string refreshTokenId = context.Request.Form[ "refresh_token" ].First();
                string clientId = context.Request.Form[ "client_id" ];
                Guid audienceId = Guid.Parse( context.Request.Form[ "audience_id" ].First() );

                var refreshToken = await _refreshTokenService.FindRefreshToken( refreshTokenId );
                if ( refreshToken == null )
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync( "Invalid refresh token." );
                    return;
                }

                await GenerateToken( context, refreshToken.Subject, audienceId, clientId );
            }
            catch
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync( "Bad request or invalid token." );
            }
        }

        private async Task GenerateToken( HttpContext context, string username, Guid audienceId, string clientId )
        {
            var user = _dbContext.Users.FirstOrDefault( x => x.Email.Equals( username ) );
            if ( user == null )
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync( "Invalid username or password." );
                return;
            }

            var audience = _dbContext.Audiences.FirstOrDefault( x => x.Id == audienceId );
            if ( audience == null )
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync( "Invalid audience." );
                return;
            }

            var client = _dbContext.Clients.FirstOrDefault( x => x.Id.Equals( clientId ) );
            if ( client == null )
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync( "Invalid client." );
                return;
            }

            var now = DateTime.UtcNow;

            _options.SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey( Encoding.ASCII.GetBytes( audience.Base64Secret ) ),
                SecurityAlgorithms.HmacSha256
            );

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[]
            {
                new Claim("user_id", user.Id.ToString().ToUpper()),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("first_name", user.FirstName),
                new Claim("last_name", user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, await _options.NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64),
                new Claim("client_id", client.Id)
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: audience.Id.ToString().ToUpper(),
                claims: claims,
                notBefore: now,
                expires: now.Add( _options.Expiration ),
                signingCredentials: _options.SigningCredentials );

            var refreshToken = new RefreshTokens()
            {
                Subject = user.Email,
                ClientId = client.Id,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes( client.RefreshTokenLifetime ),
                ProtectedTicket = jwt.ToString()
            };

            refreshToken = await _refreshTokenService.AddRefreshToken( refreshToken );

            await WriteTokenResponse( context, jwt, refreshToken.Id );
        }

        private async Task WriteTokenResponse( HttpContext context, JwtSecurityToken jwt, string refreshTokenId )
        {
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken( jwt );

            var response = new
            {
                access_token = encodedJwt,
                expires_in = ( int )_options.Expiration.TotalSeconds,
                refresh_token = refreshTokenId
            };

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync( JsonConvert.SerializeObject( response, _serializerSettings ) );
        }

        private static void ThrowIfInvalidOptions( TokenProviderOptions options )
        {
            if ( string.IsNullOrEmpty( options.Path ) )
            {
                throw new ArgumentNullException( nameof( TokenProviderOptions.Path ) );
            }

            if ( string.IsNullOrEmpty( options.Issuer ) )
            {
                throw new ArgumentNullException( nameof( TokenProviderOptions.Issuer ) );
            }

            //if ( string.IsNullOrEmpty( options.Audience ) )
            //{
            //    throw new ArgumentNullException( nameof( TokenProviderOptions.Audience ) );
            //}

            if ( options.Expiration == TimeSpan.Zero )
            {
                throw new ArgumentException( "Must be a non-zero TimeSpan.", nameof( TokenProviderOptions.Expiration ) );
            }

            if ( options.IdentityResolver == null )
            {
                throw new ArgumentNullException( nameof( TokenProviderOptions.IdentityResolver ) );
            }

            //if ( options.SigningCredentials == null )
            //{
            //    throw new ArgumentNullException( nameof( TokenProviderOptions.SigningCredentials ) );
            //}

            if ( options.NonceGenerator == null )
            {
                throw new ArgumentNullException( nameof( TokenProviderOptions.NonceGenerator ) );
            }
        }

        /// <summary>
        /// Get this datetime as a Unix epoch timestamp (seconds since Jan 1, 1970, midnight UTC).
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>Seconds since Unix epoch.</returns>
        public static long ToUnixEpochDate( DateTime date ) => new DateTimeOffset( date ).ToUniversalTime().ToUnixTimeSeconds();

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
