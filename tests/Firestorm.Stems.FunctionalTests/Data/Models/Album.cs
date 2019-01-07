using System.Collections.Generic;

namespace Firestorm.Tests.Examples.Music.Data.Models
{
    public class Album
    {
        public int AlbumID { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}