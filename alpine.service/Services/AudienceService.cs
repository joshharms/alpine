using System;
using System.Linq;
using System.Security.Cryptography;

using alpine.database.Models;
using alpine.service.Interfaces;

namespace alpine.service.Services
{
    public class AudienceService : BaseService, IAudienceService
    {
        public AudienceService( alpineContext ctx ) : base( ctx )
        { }

        public Audiences AddAudience( string name )
        {
            var audienceId = Guid.NewGuid();

            var key = new byte[ 32 ];
            RandomNumberGenerator.Create().GetBytes( key );

            var base64Secret = Convert.ToBase64String( key );

            Audiences a = new Audiences
            {
                Id = audienceId,
                Name = name,
                Base64Secret = base64Secret
            };

            db.Audiences.Add( a );
            db.SaveChanges();

            return a;
        }

        public Audiences GetAudience( Guid id )
        {
            var audience = db.Audiences.SingleOrDefault( x => x.Id == id );

            if ( null != audience )
            {
                return audience;
            }

            return null;
        }
    }
}
