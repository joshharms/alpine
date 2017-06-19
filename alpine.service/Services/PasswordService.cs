using System;
using System.Linq;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using alpine.core;
using alpine.database.Models;
using alpine.service.Interfaces;

namespace alpine.service
{
    public class PasswordService : BaseService, IPasswordService
    {
        public PasswordService( alpineContext ctx ) : base( ctx )
        { }

        private string HashPassword( string password, byte[] salt, int iteration = 20000 )
        {
            string hashed = Convert.ToBase64String( KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iteration,
                numBytesRequested: 256 / 8
            ) );

            string saltString = Convert.ToBase64String( salt );

            return string.Format( "{0}:{1}:{2}", iteration.ToString(), saltString, hashed );
        }

        private byte[] GetNewSalt()
        {
            byte[] salt = new byte[ 128 / 8 ];
            using( var rng = RandomNumberGenerator.Create() )
            {
                rng.GetBytes( salt );
            }

            return salt;
        }

        public bool PasswordStrength( string password )
        {
            if( password.Length < 8 )
            {
                throw new AlpineException( "Password length requirement has not been met" );
            }

            int number = 0, symbol = 0, upper = 0, lower = 0;

            for( var i = 0; i < password.Length; i++ )
            {
                char thisChar = password[ i ];

                if( char.IsNumber( thisChar ) )
                {
                    number++;
                }

                if( char.IsPunctuation( thisChar ) || char.IsSymbol( thisChar ) )
                {
                    symbol++;
                }

                if( char.IsUpper( thisChar ) )
                {
                    upper++;
                }

                if( char.IsLower( thisChar ) )
                {
                    lower++;
                }
            }

            if( number > 0 && symbol > 0 && upper > 0 && lower > 0 )
            {
                return true;
            }

            throw new AlpineException( "Password requirements have not been met." );
        }

        public bool SetPassword( Guid userId, string password )
        {
            if( PasswordStrength( password ) )
            {
                var user = context.Users.SingleOrDefault( x => x.Id == userId );

                if( user != null )
                {
                    var salt = GetNewSalt();
                    var hashedPassword = HashPassword( password, salt );

                    user.Password = hashedPassword;
                    user.PasswordLastUpdated = DateTime.UtcNow;

                    context.SaveChanges();

                    return true;
                }
            }

            throw new AlpineException( "Could not set password." );
        }

        public bool ChangePassword( Guid linkId, string password )
        {
            if( PasswordStrength( password ) )
            {
                var link = context.UserPasswordResetLinks.SingleOrDefault( x => x.Guid == linkId );

                if( link != null )
                {
                    if( DateTime.UtcNow < link.LinkExpiration )
                    {
                        var user = context.Users.SingleOrDefault( x => x.Id == link.UserId );

                        if( user != null )
                        {
                            var salt = GetNewSalt();
                            var hashedPassword = HashPassword( password, salt );

                            link.LinkExpiration = DateTime.UtcNow;

                            user.Password = hashedPassword;
                            user.PasswordLastUpdated = DateTime.UtcNow;

                            context.SaveChanges();

                            return true;
                        }
                    }

                    throw new AlpineException( "Link is expired." );
                }
            }

            throw new AlpineException( "Could not set password." );
        }

        public bool ValidatePassword( string email, string password )
        {
            var currentPassword = context.Users.Where( x => x.Email == email ).SingleOrDefault().Password;

            Random r = new Random();
            System.Threading.Thread.Sleep( r.Next( 300, 1500 ) );

            if( currentPassword != null )
            {
                byte[] salt = Convert.FromBase64String( currentPassword.GetSaltFromPassword() );
                var iteration = currentPassword.GetIterationFromPassword();

                var passwordInput = HashPassword( password, salt, iteration );

                if( currentPassword.GetHashFromPassword() == passwordInput.GetHashFromPassword() )
                {
                    return true;
                }
            }

            return false;
        }
    }

    static class PasswordServiceExtensions
    {
        public static int GetIterationFromPassword( this string dbPassword )
        {
            var components = dbPassword.Split( ':' );

            if( components.Length == 3 )
            {
                return Convert.ToInt32( components[ 0 ] );
            }

            return 0;
        }

        public static string GetSaltFromPassword( this string dbPassword )
        {
            var components = dbPassword.Split( ':' );

            if( components.Length == 3 )
            {
                return components[ 1 ];
            }

            return "";
        }

        public static string GetHashFromPassword( this string dbPassword )
        {
            var components = dbPassword.Split( ':' );

            if( components.Length == 3 )
            {
                return components[ 2 ];
            }

            return "";
        }
    }

}
