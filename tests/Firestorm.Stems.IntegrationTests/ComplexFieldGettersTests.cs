using System.Linq;
using System.Threading.Tasks;
using Firestorm.Stems;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Testing;
using Firestorm.Testing.Http;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class ComplexFieldGettersTests
    {
        private readonly IRestCollection _restCollection;

        public ComplexFieldGettersTests()
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

            [Get(Argument = nameof(Name))]
            public object GetComplex(string name)
            {
                return new MyComplexObject
                {
                    Name = name,
                    Look = "at this",
                    Brand = new object()
                };
            }
        }

        private class MyComplexObject
        {
            public string Name { get; set; }
            public string Look { get; set; }
            public object Brand { get; set; }
        }

        [Fact]
        public async Task Collection_GetWithComplexField_StatusOK()
        {
            RestCollectionData data = await _restCollection.QueryDataAsync(new TestCollectionQuery
            {
                SelectFields = new[] { "ID", "Complex" }
            });

            var firstComplex = (MyComplexObject)data.Items.First()["Complex"];

            Assert.Equal(TestRepositories.ArtistName, firstComplex .Name);
            Assert.Equal("at this", firstComplex.Look);
        }

        [Fact]
        public async Task Item_GetWithComplexField_StatusOK()
        {
            RestItemData data = await _restCollection.GetItem("123").GetDataAsync(new[] { "ID", "Complex" });

            var firstComplex = (MyComplexObject)data["Complex"];

            Assert.Equal(TestRepositories.ArtistName, firstComplex.Name);
            Assert.Equal("at this", firstComplex.Look);
        }

        [Fact]
        public async Task Scalar_GetComplexField_StatusOK()
        {
            var scalar = (IRestScalar) _restCollection.GetItem("123").GetField("Complex");
            var data = (MyComplexObject) await scalar.GetAsync();

            Assert.Equal(TestRepositories.ArtistName, data.Name);
            Assert.Equal("at this", data.Look);
        }
    }
}