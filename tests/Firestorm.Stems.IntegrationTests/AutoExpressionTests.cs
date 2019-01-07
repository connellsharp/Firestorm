using System.Linq;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Testing;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class AutoExpressionTests
    {
        private readonly IRestCollection _restCollection;

        public AutoExpressionTests()
        {
            var testContext = new StemTestContext();
            _restCollection = testContext.GetArtistsCollection<ArtistsStem>();
        }

        public class ArtistsStem : Stem<Artist>
        {
            [Identifier]
            [Get]
            [AutoExpr]
            public static int ID { get; }

            [Get]
            [AutoExpr]
            public static string Name { get; set; }
        }

        [Fact]
        public async Task Item_GetName_CorrectName()
        {
            var name = await _restCollection.GetItem("123").GetScalar("Name").GetAsync() as string;

            Assert.Equal(TestRepositories.ArtistName, name);
        }
    }
}