using System;

namespace alpine.service.Interfaces
{
    public interface IPasswordService
    {
        bool PasswordStrength( string password );
        bool SetPassword( Guid userId, string password );
        bool ChangePassword( Guid linkId, string password );
        bool ValidatePassword( string email, string password, bool countAgainstAccessFailed = true );
    }
}
