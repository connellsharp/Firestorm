using System;
using Firestorm.Endpoints;

namespace Firestorm.Tests.Unit.Stems.Roots
{
    public class TestEndpointContext : IRestEndpointContext
    {
        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        public RestEndpointConfiguration Configuration { get; }

        public IRestUser User { get; set; }

        public event EventHandler OnDispose;
    }
}
