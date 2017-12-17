using Firestorm.Engine.Defaults;

namespace Firestorm.Tests.Unit.Engine.Models
{
    public class ArtistMemoryRepository : MemoryRepository<Artist>
    {
        public ArtistMemoryRepository()
            : base(TestRepositories.GetArtists())
        {
            
        }
    }
}