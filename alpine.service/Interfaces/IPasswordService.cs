using System;
using System.Collections.Generic;
using System.Linq;

namespace alpine.service.Interfaces
{
    public interface IPasswordService
    {
        bool PasswordStrength( string password );
        bool SetPassword( Guid userId, string password );
        bool ComparePasswords( string email, string password );
    }
}
