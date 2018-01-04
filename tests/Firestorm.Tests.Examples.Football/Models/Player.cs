﻿using System.Collections.Generic;

namespace Firestorm.Tests.Examples.Football.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SquadNumber { get; set; }

        public Team Team { get; set; }

        public ICollection<Goal> Goals { get; set; }
    }
}