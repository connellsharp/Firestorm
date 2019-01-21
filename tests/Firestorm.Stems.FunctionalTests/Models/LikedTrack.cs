using System.ComponentModel.DataAnnotations.Schema;

namespace Firestorm.Stems.FunctionalTests.Data.Models
{
    public class LikedTrack
    {
        public int LikedTrackID { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        public int TrackID { get; set; }

        [ForeignKey("TrackID")]
        public Track Track { get; set; }
    }
}