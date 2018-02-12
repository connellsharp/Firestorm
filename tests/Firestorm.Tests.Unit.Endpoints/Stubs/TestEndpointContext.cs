using System;
using System.Collections.Generic;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;

namespace Firestorm.Tests.Unit.Endpoints.Stubs
{
    public class TestEndpointContext : IRestEndpointContext, IRestUser, IRestCollectionQuery
    {
        public const string TestUsername = "TestUsername";
        public const string TestRole = "TestRole";

        public RestEndpointConfiguration Configuration { get; } = new DefaultRestEndpointConfiguration();

        public IRestUser User => this;

        public IRestCollectionQuery GetQuery() => this;

        public string Username { get; set; } = TestUsername;

        public bool IsAuthenticated { get; } = true;

        public bool IsInRole(string role)
        {
            return role == TestRole;
        }

        public IEnumerable<string> SelectFields { get; set; }

        public IEnumerable<FilterInstruction> FilterInstructions { get; }

        public IEnumerable<SortInstruction> SortInstructions { get; set; }

        public PageInstruction PageInstruction { get; set; }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}