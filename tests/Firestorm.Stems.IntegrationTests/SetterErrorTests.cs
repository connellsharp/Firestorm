using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Testing;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class SetterErrorTests
    {
        private readonly IRestCollection _restCollection;

        public SetterErrorTests()
        {
            var testContext = new StemTestContext();
            _restCollection = testContext.GetArtistsCollection<ArtistsStem>();
        }

        public class ArtistsStem : Stem<Artist>
        {
            [Identifier, Get]
            public static Expression Id => Expression(a => a.ID);

            [Get, Set]
            public static Expression Url => Expression(a => "/artists/" + a.ID);
        }

        [Fact]
        public async Task Item_GetName_CorrectName()
        {
            var ack = await _restCollection.AddAsync(new
            {
                id = 432
            });

            Assert.NotNull(ack);
        }
    }
}