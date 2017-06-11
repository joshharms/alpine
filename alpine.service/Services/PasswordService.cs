using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using alpine.core;
using alpine.database.Models;
using alpine.service.Interfaces;

namespace alpine.service
{
    public class PasswordService : BaseService, IPasswordService
    {
        public PasswordService( alpineContext context ) : base( context )
        { }

        private string HashPassword( string password, byte[] salt, int iteration )
        {
            string hashed = Convert.ToBase64String( KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iteration,
                numBytesRequested: 256 / 8
            ) );

            return string.Format( "{0}:{1}:{2}", iteration.ToString(), salt, hashed );
        }

        private byte[] GetNewSalt()
        {
            byte[] salt = new byte[ 128 / 8 ];
            using ( var rng = RandomNumberGenerator.Create() )
            {
                rng.GetBytes( salt );
            }

            return salt;
        }

        public bool PasswordStrength( string password )
        {
            if ( password.Length < 8 )
            {
                throw new AlpineException( "Password length requirement has not been met" );
            }

            int number = 0;
            int symbol = 0;
            int upper = 0;
            int lower = 0;

            for ( var i = 0; i < password.Length; i++ )
            {
                char thisChar = password[ i ];

                if ( char.IsNumber( thisChar ) )
                {
                    number++;
                }

                if ( char.IsSymbol( thisChar ) )
                {
                    symbol++;
                }

                if ( char.IsUpper( thisChar ) )
                {
                    upper++;
                }

                if ( char.IsLower( thisChar ) )
                {
                    lower++;
                }
            }

            if ( number > 0 && symbol > 0 && upper > 0 && lower > 0 )
            {
                return true;
            }

            throw new AlpineException( "Password requirements have not been met." );
        }

        public bool SetPassword( Guid userId, string password )
        {
            return true;
        }

        public bool ComparePasswords( string email, string password )
        {
            var currentPassword = context.Users.Where( x => x.Email == email ).SingleOrDefault().Password;

            if ( currentPassword != null )
            {

            }

            return Test();
        }

        private bool Test()
        {
            return false;
        }
    }
}
