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
        public UserService( alpineContext ctx, ApiKeyAccessor key ) : base( ctx, key )
        { }

        public List<UserSummary> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}