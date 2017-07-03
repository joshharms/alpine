using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using alpine.database.Models;

namespace alpine.service.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokens> AddRefreshToken( RefreshTokens token );
        Task<bool> RemoveRefreshToken( string refreshTokenId );
        Task<bool> RemoveRefreshToken( RefreshTokens refreshToken );
        Task<RefreshTokens> FindRefreshToken( string refreshTokenId );
        List<RefreshTokens> GetAllRefreshTokens();
    }
}
