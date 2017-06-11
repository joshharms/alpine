using System;

using Microsoft.EntityFrameworkCore;

using alpine.database.Models;

namespace alpine.service
{
    public class BaseService : IDisposable
    {
        protected readonly alpineContext context;

        public BaseService( alpineContext alpineContext )
        {
            context = alpineContext;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
