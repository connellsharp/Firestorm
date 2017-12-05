using System;
using System.Collections.Generic;

namespace Firestorm.Tests.Unit
{
    [Obsolete("Tests that use this should probably be refactored to be more data-driven tests.")]
    public static class TestRepositories
    {
        public static IEnumerable<Artist> GetArtists()
        {
            yield return new Artist(123, ArtistName)
            {
                Albums = new List<Album>
                {
                    new Album { ID = 1, Name = AlbumName }
                }
            };
        }

        public static string AlbumName
        {
            get { return "Split the Atom"; }
        }

        public static string ArtistName
        {
            get { return "Noisia"; }
        }
    }
}