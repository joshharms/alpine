using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using alpine.core;

namespace alpine.api.Controllers
{
    public class DataController : Controller
    {
        protected AuthenticationToken GetToken()
        {
            var context = HttpContext.User.Claims.ToList();

            AuthenticationToken token = new AuthenticationToken();

            if ( context.Count() > 0 )
            {
                token.userId = Guid.Parse( context[ 0 ].Value );
                token.email = context[ 1 ].Value;
                token.firstName = context[ 2 ].Value;
                token.lastName = context[ 3 ].Value;
                token.role = context[ 4 ].Value;
                //token.organizationId = Guid.Parse( context[ 5 ].Value );
                token.avatar = context[ 6 ].Value;
            }

            return token;
        }

        protected JsonResult SuccessMessage( object data = null )
        {
            return Json( new AlpineCreateResponse().Success( 200, data ) );
        }
    }
}
