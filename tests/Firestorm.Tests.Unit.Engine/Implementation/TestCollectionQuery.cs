using System.Collections.Generic;

namespace Firestorm.Tests.Unit.Engine.Implementation
{
    public class TestCollectionQuery : IRestCollectionQuery
    {
        public IEnumerable<string> SelectFields { get; set; }

        public IEnumerable<FilterInstruction> FilterInstructions { get; set; }

        public IEnumerable<SortIntruction> SortIntructions { get; set; }

        public PageInstruction PageInstruction { get; set; }
    }
}