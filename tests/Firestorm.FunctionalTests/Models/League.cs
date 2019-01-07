using System.Collections.Generic;

namespace Firestorm.Tests.Examples.Football.Models
{
    public class League
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}