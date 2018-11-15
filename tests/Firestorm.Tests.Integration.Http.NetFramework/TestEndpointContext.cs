using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;
using Firestorm.Host;

namespace Firestorm.Tests.Integration.Http.NetFramework
{
    public class TestEndpointContext : IRestEndpointContext
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