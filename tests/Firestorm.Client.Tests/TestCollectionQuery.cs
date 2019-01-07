using System.Collections.Generic;

namespace Firestorm.Client.Tests
{
    public class TestCollectionQuery : IRestCollectionQuery
    {
        public IEnumerable<string> SelectFields { get; set; }

        public IEnumerable<FilterInstruction> FilterInstructions { get; set; }

        public IEnumerable<SortInstruction> SortInstructions { get; set; }

        public PageInstruction PageInstruction { get; set; }
    }
}