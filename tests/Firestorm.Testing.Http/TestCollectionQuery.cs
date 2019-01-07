using System.Collections.Generic;

namespace Firestorm.Tests.Integration.Http.NetFramework
{
    public class TestCollectionQuery : IRestCollectionQuery
    {
        public IEnumerable<string> SelectFields { get; set; }
        public IEnumerable<FilterInstruction> FilterInstructions { get; }
        public IEnumerable<SortInstruction> SortInstructions { get; }
        public PageInstruction PageInstruction { get; }
    }
}