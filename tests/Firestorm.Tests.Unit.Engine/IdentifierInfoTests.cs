using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Tests.Unit.Engine.Models;
using Xunit;

namespace Firestorm.Tests.Unit.Engine
{
    public class IdentifierInfoTests
    {
        [Fact]
        public void CheckArtistIDConventionKey()
        {
            var artist = new Artist(123, TestRepositories.ArtistName);
            var keyInfo = new IDConventionIdentifierInfo<Artist>();
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