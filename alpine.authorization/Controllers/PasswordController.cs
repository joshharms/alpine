using System;

using Microsoft.AspNetCore.Mvc;

using alpine.service.Interfaces;

namespace alpine.authorization.Controllers
{
    [Route( "api/[controller]" )]
    public class PasswordController : Controller
    {
        private readonly IPasswordService _passwordService;

        public PasswordController( IPasswordService passwordService )
        {
            _passwordService = passwordService;
        }

        // POST api/password/reset
        [HttpPut]
        [Route( "reset" )]
        public bool Put( string email )
        {
            throw new NotImplementedException();
        }

        // POST api/password/set
        [HttpPost]
        [Route( "set" )]
        public bool Post( string password, Guid id )
        {
            return _passwordService.ChangePassword( id, password );
        }
    }
}
