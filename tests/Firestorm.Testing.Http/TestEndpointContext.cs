﻿using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;
using Firestorm.Host;

namespace Firestorm.Testing.Http
{
    public class TestEndpointContext : IEndpointContext
    {
        public RestEndpointConfiguration Configuration { get; } = new DefaultRestEndpointConfiguration();
        
        public IRequestContext Request { get; } = new TestRequestContext();
    }

    public class TestRequestContext : IRequestContext
    {
        public IRestUser User { get; }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}