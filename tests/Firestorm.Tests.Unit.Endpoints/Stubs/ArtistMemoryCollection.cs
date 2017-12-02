using System.Collections.Generic;
using System.Linq;
using Firestorm.Tests.Models;

namespace Firestorm.Tests.Endpoints.Models
{
    //[StartRestEndpoint("artists")]
    internal class ArtistMemoryCollection : MemoryRestCollection<Artist>
    {
        protected override List<Artist> Entities { get; } = TestRepositories.GetArtists().ToList();

        protected override string GetIdentifier(Artist entity)
        {
            return entity.ID.ToString();
        }
    }
}