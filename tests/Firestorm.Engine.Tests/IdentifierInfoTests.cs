using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Tests.Models;
using Xunit;

namespace Firestorm.Engine.Tests
{
    public class IdentifierInfoTests
    {
        [Fact]
        public void CheckArtistIDConventionKey()
        {
            var artist = new Artist(123, TestRepositories.ArtistName);
            var keyInfo = new IdConventionIdentifierInfo<Artist>();
            var key = keyInfo.GetValue(artist);
            Assert.Equal(123, key);
        }

        private static FieldDictionary<Artist> GetArtistFieldMappings()
        {
            return new FieldDictionary<Artist>
            {
                {"id", a => a.ID},
                {"name", a => a.Name}
            };
        }
    }
}