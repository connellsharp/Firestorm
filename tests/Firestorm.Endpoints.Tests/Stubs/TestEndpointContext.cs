using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Configuration;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Tests.Stubs
{
    public class TestEndpointContext : IEndpointContext
    {
        public IRequestContext Request => new TestRequestContext();

        public EndpointConfiguration Configuration { get; } = new DefaultEndpointConfiguration();
    }

    public class TestRequestContext : IRequestContext, IRestUser
    {
        public const string TestUsername = "TestUsername";
        public const string TestRole = "TestRole";

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