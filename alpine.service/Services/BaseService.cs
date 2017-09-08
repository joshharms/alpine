using System;

using Microsoft.EntityFrameworkCore;

using alpine.core;
using alpine.database.Models;

namespace alpine.service
{
    public class BaseService : IDisposable
    {
        public readonly alpineContext context;
        public readonly AuthenticationToken token;

        public BaseService( alpineContext alpineContext )
        {
            context = alpineContext;
        }

        public BaseService( alpineContext alpineContext, AuthenticationTokenAccessor authenticationTokenAccessor )
        {
            context = alpineContext;
            token = authenticationTokenAccessor.token;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
