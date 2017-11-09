using System.Threading.Tasks;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Queryable;
using Firestorm.Tests.Engine.Models;
using Firestorm.Tests.Models;
using Xunit;

namespace Firestorm.Tests.Engine.Fields
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
