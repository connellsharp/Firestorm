using System.Collections.Generic;

namespace Firestorm.Tests.Unit.Client
{
    public class TestCollectionQuery : IRestCollectionQuery
    {
        public IEnumerable<string> SelectFields { get; set; }

        public IEnumerable<FilterInstruction> FilterInstructions { get; set; }

        public IEnumerable<SortInstruction> SortInstructions { get; set; }

        public PageInstruction PageInstruction { get; set; }
    }
}