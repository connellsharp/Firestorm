using System.Collections.Generic;

namespace Firestorm.FunctionalTests.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FoundedYear { get; set; }

        public ICollection<Player> Players { get; set; }

        public League League { get; set; }

        public ICollection<FixtureTeam> Fixtures { get; set; }
    }
}
