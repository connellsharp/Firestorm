using System;
using System.Collections.Generic;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;

namespace Firestorm.Tests.Integration.Http.NetFramework
{
    public class TestEndpointContext : IRestEndpointContext
    {
        public void Dispose()
        {
        }

        public RestEndpointConfiguration Configuration { get; } = new DefaultRestEndpointConfiguration();

        public IRestUser User { get; }

        public event EventHandler OnDispose;
    }

    public class TestCollectionQuery : IRestCollectionQuery
    {
        public IEnumerable<string> SelectFields { get; set; }
        public IEnumerable<FilterInstruction> FilterInstructions { get; }
        public IEnumerable<SortInstruction> SortInstructions { get; }
        public PageInstruction PageInstruction { get; }
    }
}