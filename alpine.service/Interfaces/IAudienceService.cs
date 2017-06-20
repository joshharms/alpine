using System;

using alpine.database.Models;

namespace alpine.service.Interfaces
{
    public interface IAudienceService
    {
        Audiences AddAudience( string name );
        Audiences GetAudience( Guid id );
    }
}
