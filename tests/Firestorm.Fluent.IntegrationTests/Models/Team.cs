using System.Collections.Generic;

namespace Firestorm.Fluent.IntegrationTests.Models
{
    public class Team
    {
        public int Division { get; set; }

        public int Position { get; set; }

        public string Name { get; set; }

        public IList<Player> Players { get; set; }
    }
}