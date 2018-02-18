using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;

namespace Firestorm.Tests.Unit.Endpoints.Stubs
{
    public class TestEndpointContext : IRestEndpointContext, IRestUser
    {
        public const string TestUsername = "TestUsername";
        public const string TestRole = "TestRole";

        public RestEndpointConfiguration Configuration { get; } = new DefaultRestEndpointConfiguration();

        public IRestUser User => this;

        public string Username { get; } = TestUsername;

        public bool IsAuthenticated { get; } = true;

        public bool IsInRole(string role)
        {
            return role == TestRole;
        }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}