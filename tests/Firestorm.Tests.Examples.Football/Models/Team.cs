using System;
using System.Collections.Generic;
using System.Text;

namespace Firestorm.Tests.Examples.Football.Models
{
    public class Team
    {
        public string Name { get; set; }

        public int FoundedYear { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
