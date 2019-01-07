using System.Threading.Tasks;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Queryable;
using Firestorm.Engine.Tests.Models;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Engine.Tests.Queryable
{
    public class QueryableFieldSetterTests
    {
        public QueryableFieldSetterTests()
        {
            Setter = new QueryableFieldSetter<Artist>(new CodedArtistEntityContext(null));
        }

        private QueryableFieldSetter<Artist> Setter { get; set; }

        [Fact]
        public async Task SetsName()
        {
            Artist artist = new Artist();
            var loadedItem = new PostedNewItem<Artist>(artist);
            const string setToValue = "Test";

            await Setter.SetMappedValuesAsync(loadedItem, new RestItemData(new { name = setToValue }));

            Assert.Equal(artist.Name, setToValue);
        }
    }
}
