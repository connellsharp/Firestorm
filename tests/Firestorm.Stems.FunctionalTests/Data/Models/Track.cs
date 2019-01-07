using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firestorm.Stems.FunctionalTests.Data.Models
{
    public class Track
    {
        public int TrackID { get; set; }

        public string Title { get; set; }

        public int ArtistID { get; set; }

        [ForeignKey("ArtistID")]
        public virtual Artist Artist { get; set; }
        
        public virtual ICollection<LikedTrack> LikedByUsers { get; set; }
    }
}