using System;
using System.Collections.Generic;
using Firestorm.Testing.Models;

namespace Firestorm.Testing.Data
{
    public class ExampleDataSets
    {
        public static IEnumerable<Artist> GetArtists()
        {
            // Note the IDs are not here due to differences in EF.
            // There's some horrible code in EF6 tests to add the IDs manually.
            
            return new List<Artist>
            {
                new Artist { Name = "Eminem", StartDate = new DateTime(2007, 05, 01) },
                new Artist { Name = "Noisia", StartDate = new DateTime(1995, 01, 01) },
                new Artist { Name = "Periphery", StartDate = new DateTime(2005, 01, 01) },
                new Artist { Name = "Infected Mushroom", StartDate = new DateTime(1989, 01, 01) }
            };
        }
    }
}