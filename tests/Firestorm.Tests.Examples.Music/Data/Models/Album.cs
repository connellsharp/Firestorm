using System.Collections.Generic;

namespace Firestorm.Tests.Examples.Data.Models
{
    public class Album
    {
        public int AlbumID { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}