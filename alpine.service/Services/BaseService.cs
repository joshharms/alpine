﻿using System;

using Microsoft.EntityFrameworkCore;

using alpine.core;
using alpine.database.Models;

namespace alpine.service
{
    public class BaseService : IDisposable
    {
        public readonly alpineContext context;
        public readonly AuthenticationToken token;
        public readonly string _apiKey;

        public BaseService( alpineContext alpineContext )
        {
            context = alpineContext;
        }

        public BaseService( alpineContext alpineContext, ApiKeyAccessor apiKeyAccessor )
        {
            context = alpineContext;
            _apiKey = apiKeyAccessor.ApiKey;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
