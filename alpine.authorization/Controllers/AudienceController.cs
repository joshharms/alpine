using System;

using Microsoft.AspNetCore.Mvc;

using alpine.database.Models;
using alpine.service.Interfaces;

namespace alpine.authorization.Controllers
{
    [Route( "api/[controller]" )]
    public class AudienceController : Controller
    {
        private readonly IAudienceService _audienceService;

        public AudienceController( IAudienceService audienceService )
        {
            _audienceService = audienceService;
        }

        // GET api/audience/5
        [HttpGet( "{id}" )]
        public Audiences Get( Guid id )
        {
            return _audienceService.GetAudience( id );
        }

        // POST api/audience
        [HttpPost]
        public Audiences Post( string name )
        {
            return _audienceService.AddAudience( name );
        }
    }
}
