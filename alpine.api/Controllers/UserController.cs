using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using alpine.service.Interfaces;
using alpine.service.Services;

namespace alpine.api.Controllers
{
    [Route( "api/[controller]" )]
    public class UserController : DataController
    {
        private readonly IUserService _users;

        public UserController( IUserService users )
        {
            _users = users;
        }

        // GET api/user
        [HttpGet]
        [Authorize]
        public JsonResult Get()
        {
            var a = GetToken();
            return SuccessMessage( _users.GetUsers() );
        }
    }
}
