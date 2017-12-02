using System.Collections.Generic;
using Firestorm.Engine.Defaults;
using Firestorm.Tests.Models;

namespace Firestorm.Tests.Integration.Http.Base.Models
{
    // TODO all these were taking from the Engine Unit tests. Probably should stay here and be removed from the unit tests?

    public class ArtistMemoryRepository : MemoryRepository<Artist>
    {
        protected override IEnumerable<Artist> LoadInitialRepository()
        {
            return TestRepositories.GetArtists();
        }
    }
}