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

        public async Task<RefreshTokens> AddRefreshToken( RefreshTokens token )
        {
            var existingToken = db.RefreshTokens.SingleOrDefault( x => x.Subject == token.Subject && x.ClientId == token.ClientId );

            if ( existingToken != null )
            {
                var result = await RemoveRefreshToken( existingToken );
            }

            token.Id = Guid.NewGuid().ToString( "n" );
            db.RefreshTokens.Add( token );
            await db.SaveChangesAsync();

            return token;

        }

        public async Task<bool> RemoveRefreshToken( string refreshTokenId )
        {
            var refreshToken = await db.RefreshTokens.FindAsync( refreshTokenId );

            if ( refreshToken != null )
            {
                db.RefreshTokens.Remove( refreshToken );
                return await db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken( RefreshTokens refreshToken )
        {
            db.RefreshTokens.Remove( refreshToken );
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<RefreshTokens> FindRefreshToken( string refreshTokenId )
        {
            var refreshToken = await db.RefreshTokens.FindAsync( refreshTokenId );
            return refreshToken;
        }

        public List<RefreshTokens> GetAllRefreshTokens()
        {
            return db.RefreshTokens.ToList();
        }
    }
}
