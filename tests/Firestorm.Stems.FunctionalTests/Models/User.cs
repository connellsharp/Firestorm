using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firestorm.Stems.FunctionalTests.Models
{
    public class User
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public int? MyArtistID { get; set; }

        [ForeignKey("MyArtistID")]
        public Artist MyArtist { get; set; }

        public virtual ICollection<LikedTrack> LikedTracks { get; set; }
    }
}