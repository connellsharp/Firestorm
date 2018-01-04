using System;
using System.Collections.Generic;

namespace Firestorm.Tests.Examples.Football.Models
{
    public class Fixture
    {
        public int Id { get; set; }

        public League League { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Goal> Goals { get; set; }
    }
}