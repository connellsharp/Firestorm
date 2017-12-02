using System;
using System.Collections.Generic;
using Firestorm.Endpoints;

namespace Firestorm.Tests.Integration.Http.NetFramework
{
    public class TestEndpointContext : IRestEndpointContext, IRestCollectionQuery
    {
        public void Dispose()
        {
        }

        public RestEndpointConfiguration Configuration { get; }
        public IRestUser User { get; }

        public IRestCollectionQuery GetQuery()
        {
            return this;
        }

        public event EventHandler OnDispose;

        public IEnumerable<string> SelectFields { get; set; }
        public IEnumerable<FilterInstruction> FilterInstructions { get; }
        public IEnumerable<SortIntruction> SortIntructions { get; }
        public PageInstruction PageInstruction { get; }
    }
}