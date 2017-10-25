using System.Collections.Generic;
using Firestorm.Engine;
using Firestorm.Tests.Models;

namespace Firestorm.Tests.Engine.Models
{
    public class ArtistMemoryRepository : MemoryRepository<Artist>
    {
        protected override IEnumerable<Artist> LoadInitialRepository()
        {
            return TestRepositories.GetArtists();
        }
    }
}