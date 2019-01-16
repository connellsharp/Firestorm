using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.IntegrationTests.Helpers;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.IntegrationTests
{
    public class SetterErrorTests
    {
        public class ArtistsStem : Stem<Artist>
        {
            [Identifier, Get]
            public static Expression Id => Expression(a => a.ID);

            [Get, Set]
            public static Expression Url => Expression(a => "/artists/" + a.ID);
        }

        [Fact]
        public async Task IncorrectSetterExpression_AddAnything_ExceptionContainsEnoughDetail()
        {
            var testContext = new StemTestContext();
            
            Task AddItemAsync()
            {
                var restCollection = testContext.GetArtistsCollection<ArtistsStem>();
                return restCollection.AddAsync(new {id = 432});
            }

            var ex = await Assert.ThrowsAsync<StemAttributeSetupException>(AddItemAsync);
            Assert.Contains("setter", ex.InnerException.Message);
        }
    }
}