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
        private List<UserSummary> _users;

        public UserService( alpineContext ctx, AuthenticationTokenAccessor auth ) : base( ctx, auth )
        {
            _users = new List<UserSummary>();
        }

        public List<UserSummary> GetUsers()
        {
            List<Users> users = new List<Users>();

            if ( token.role == AlpineRoles.RoleType.PlatformAdministrator.ToDisplayString() )
            {
                users = db.Users.ToList();
            }

            foreach ( var user in users )
            {
                UserSummary u = new UserSummary();
                u.id = user.Id;
                u.active = user.Active;
                u.email = user.Email;
                u.phoneNumber = user.PhoneNumber;
                //u.avatar = user.Avatar;
                u.createdDate = user.CreatedDate.ToUtcString();

                _users.Add( u );
            }

            return _users;

        }
    }
}