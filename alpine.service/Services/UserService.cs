using System;
using System.Collections.Generic;
using System.Linq;

using alpine.core;
using alpine.database.Models;
using alpine.infrastructure.Users;
using alpine.service.Interfaces;

namespace alpine.service.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService( alpineContext ctx, AuthenticationTokenAccessor auth ) : base( ctx, auth )
        { }

        public List<UserSummary> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}