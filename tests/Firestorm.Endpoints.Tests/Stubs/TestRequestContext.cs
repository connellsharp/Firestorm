using System;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Tests.Stubs
{
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