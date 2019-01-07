using System.Linq;
using Firestorm.Engine.Defaults;
using Firestorm.Tests.Unit;

namespace Firestorm.Testing.Http.Models
{
    public class ArtistMemoryRepository : MemoryRepository<Artist>
    {
        public ArtistMemoryRepository()
            :base(TestRepositories.GetArtists().ToList())
        {
            
        }
    }
}