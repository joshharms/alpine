using System;

using Microsoft.EntityFrameworkCore;

using alpine.core;
using alpine.database.Models;

namespace alpine.service
{
    public class BaseService : IDisposable
    {
        public readonly alpineContext db;
        public readonly AuthenticationToken token;

        protected string PlatformAdminDisplay = AlpineRoles.RoleType.PlatformAdministrator.ToDisplayString();
        protected string AdministratorDisplay = AlpineRoles.RoleType.Administrator.ToDisplayString();

        public BaseService( alpineContext alpineContext )
        {
            db = alpineContext;
        }

        public BaseService( alpineContext alpineContext, AuthenticationTokenAccessor authenticationTokenAccessor )
        {
            db = alpineContext;
            token = authenticationTokenAccessor.token;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
