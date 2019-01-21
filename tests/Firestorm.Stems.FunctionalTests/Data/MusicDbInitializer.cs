using System;
using System.Collections.Generic;
using Firestorm.Stems.FunctionalTests.Data.Models;

namespace Firestorm.Stems.FunctionalTests.Data
{
    public static class MusicDbInitializer
    {
        public static void Initialize(MusicDbContext context)
        {
            context.Artists.AddRange(new List<Artist> {
                new Artist { ArtistID = 1, Name = "Eminem", StartDate = new DateTime(2007, 05, 01) },
                new Artist { ArtistID = 2, Name = "Noisia", StartDate = new DateTime(1995, 01, 01) },
                new Artist { ArtistID = 3, Name = "Periphery", StartDate = new DateTime(2005, 01, 01) },
                new Artist { ArtistID = 4, Name = "Infected Mushroom", StartDate = new DateTime(1989, 01, 01) }
            });

            context.Tracks.AddRange(new List<Track>
            {
                new Track { TrackID = 1, ArtistID = 2, Title = "Dustup" },
                new Track { TrackID = 2, ArtistID = 2, Title = "Asteroids" },
                new Track { TrackID = 3, ArtistID = 2, Title = "Dead Limit" },
                new Track { TrackID = 4, ArtistID = 2, Title = "Machine Gun" },
                new Track { TrackID = 5, ArtistID = 3, Title = "Flatline" },
                new Track { TrackID = 6, ArtistID = 3, Title = "Stranger Things" },
                new Track { TrackID = 7, ArtistID = 3, Title = "Alpha" },
                new Track { TrackID = 8, ArtistID = 4, Title = "Heavyweight" },
                new Track { TrackID = 9, ArtistID = 4, Title = "Vicious Delicious" }
            });

            context.Albums.AddRange(new List<Album>
            {
                new Album { AlbumID = 1, Tracks = new List<Track> { context.Tracks.Find(1), context.Tracks.Find(4) } },
                new Album { AlbumID = 2, Tracks = new List<Track> { context.Tracks.Find(2) } },
                new Album { AlbumID = 3, Tracks = new List<Track> { context.Tracks.Find(3) } }
            });

            context.Users.AddRange(new List<User>
            {
                new User { UserID = 1, Username = "Me", MyArtistID = 1 }
            });

            context.LikedTracks.AddRange(new List<LikedTrack>
            {
                new LikedTrack { UserID = 1, TrackID = 1 },
                new LikedTrack { UserID = 1, TrackID = 3 },
                new LikedTrack { UserID = 1, TrackID = 5 },
                new LikedTrack { UserID = 1, TrackID = 7 },
                new LikedTrack { UserID = 1, TrackID = 9 }
            });

            context.SaveChanges();
        }
    }
}