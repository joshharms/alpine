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
    [Route( "v1/[controller]" )]
    public class UserController : DataController
    {
        private readonly IUserService _users;

        public UserController( IUserService users )
        {
            _users = users;
        }

        // GET v1/user
        [HttpGet]
        [Authorize]
        public JsonResult Get()
        {
            return SuccessMessage( _users.GetUsers() );
        }
    }
}
