using System;
using System.Collections.Generic;

namespace Firestorm.Tests.Examples.Football.Models
{
    public class Fixture
    {
        public int Id { get; set; }

        public League League { get; set; }

        public ICollection<FixtureTeam> Teams { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Goal> Goals { get; set; }
    }

    public class FixtureTeam
    {
        public Team Team { get; set; }

        public int FixtureId { get; set; }

        public Fixture Fixture { get; set; }

        public bool IsHome { get; set; }
    }
}