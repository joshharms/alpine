using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using alpine.database.Models;
using alpine.service.Interfaces;

namespace alpine.service.Services
{
    public class RefreshTokenService : BaseService, IRefreshTokenService
    {
        public RefreshTokenService( alpineContext ctx ) : base( ctx )
        { }

        public async Task<bool> AddRefreshToken( RefreshTokens token )
        {
            var existingToken = context.RefreshTokens.SingleOrDefault( x => x.Subject == token.Subject && x.ClientId == token.ClientId );

            if( existingToken != null )
            {
                var result = await RemoveRefreshToken( existingToken );
            }

            context.RefreshTokens.Add( token );

            return await context.SaveChangesAsync() > 0;

        }

        public async Task<bool> RemoveRefreshToken( string refreshTokenId )
        {
            var refreshToken = await context.RefreshTokens.FindAsync( refreshTokenId );

            if( refreshToken != null )
            {
                context.RefreshTokens.Remove( refreshToken );
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken( RefreshTokens refreshToken )
        {
            context.RefreshTokens.Remove( refreshToken );
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshTokens> FindRefreshToken( string refreshTokenId )
        {
            var refreshToken = await context.RefreshTokens.FindAsync( refreshTokenId );
            return refreshToken;
        }

        public List<RefreshTokens> GetAllRefreshTokens()
        {
            return context.RefreshTokens.ToList();
        }
    }
}
