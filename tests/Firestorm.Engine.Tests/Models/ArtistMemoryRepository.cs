using System.Linq;
using Firestorm.Engine.Defaults;

namespace Firestorm.Engine.Tests.Models
{
    public class ArtistMemoryRepository : MemoryRepository<Artist>
    {
        public ArtistMemoryRepository()
            : base(TestRepositories.GetArtists().ToList())
        {
            
        }
    }
}