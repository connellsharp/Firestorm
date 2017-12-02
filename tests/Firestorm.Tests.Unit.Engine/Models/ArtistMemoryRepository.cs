using System.Collections.Generic;
using Firestorm.Engine.Defaults;

namespace Firestorm.Tests.Unit.Engine.Models
{
    public class ArtistMemoryRepository : MemoryRepository<Artist>
    {
        protected override IEnumerable<Artist> LoadInitialRepository()
        {
            return TestRepositories.GetArtists();
        }
    }
}