using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine;
using Firestorm.Stems;
using Firestorm.Stems.Attributes.Basic.Attributes;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Tests.Functionality.Stems.Helpers;
using Firestorm.Tests.Unit;
using Xunit;

namespace Firestorm.Tests.Functionality.Stems
{
    public class CanAddItemTests
    {
        private readonly IRestCollection _restCollection;
        private readonly CanAddItemValue _canAddItemValue;

        public CanAddItemTests()
        {
            var testContext = new StemTestContext();
            _canAddItemValue = new CanAddItemValue();
            testContext.TestDependencyResolver.Add(_canAddItemValue);
            _restCollection = testContext.GetArtistsCollection<ArtistsStem>();
        }

        public class ArtistsStem : Stem<Artist>
        {
            private readonly CanAddItemValue _canAddItemValue;

            public ArtistsStem(CanAddItemValue canAddItemValue)
            {
                _canAddItemValue = canAddItemValue;
            }
            
            [Identifier, Get]
            public static Expression Id
            {
                get { return Expression(a => a.ID); }
            }
            
            [Get, Set]
            public static Expression Name
            {
                get { return Expression(a => a.Name); }
            }

            public override bool CanAddItem()
            {
                return _canAddItemValue.Value;
            }
        }

        public class CanAddItemValue
        {
            public bool Value { get; set; }
        }

        [Fact]
        public async Task CanAddItemTrue_Add_DoesntThrow()
        {
            _canAddItemValue.Value = true;

            var ack = await _restCollection.AddAsync(new
            {
                Name = "test"
            });
        }

        [Fact]
        public async Task CanAddItemFalse_Add_Throws()
        {
            _canAddItemValue.Value = false;

            await Assert.ThrowsAsync<NotAuthorizedForItemException>(async delegate
            {
                var ack = await _restCollection.AddAsync(new
                {
                    Name = "test"
                });
            });
        }
    }
}