using System;
using System.Collections.Generic;

namespace Firestorm.Stems.FunctionalTests.Models
{
    public class Artist
    {
        public int ArtistID { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}