using System;
using System.Collections.Generic;
using Firestorm.Tests.Integration.Data.Base.Models;

namespace Firestorm.Tests.Integration.Data.Base
{
    public class ExampleDataSets
    {
        public static IEnumerable<Artist> GetArtists()
        {
            return new List<Artist> {
                new Artist { Name = "Eminem", StartDate = new DateTime(2007, 05, 01) },
                new Artist { Name = "Noisia", StartDate = new DateTime(1995, 01, 01) },
                new Artist { Name = "Periphery", StartDate = new DateTime(2005, 01, 01) },
                new Artist { Name = "Infected Mushroom", StartDate = new DateTime(1989, 01, 01) }
            };
        }
    }
}