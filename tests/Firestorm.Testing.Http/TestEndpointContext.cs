using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Endpoints.Web;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Testing.Http
{
    public class TestEndpointContext : IEndpointContext
    {
        public RestEndpointConfiguration Configuration { get; } = new DefaultRestEndpointConfiguration();
        
        public IRequestContext Request { get; } = new TestRequestContext();
    }

    public class TestRequestContext : IRequestContext
    {
        public const string TestUsername = "TestUsername";
        public const string TestRole = "TestRole";
        
        public IRestUser User { get; }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}