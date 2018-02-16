using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.Web;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Responses
{
    public class WrapResourceObjectTests
    {
        private readonly Fixture _fixture;

        public WrapResourceObjectTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Fact]
        public void SuccessBoolean_ResourceResponse_HasWrappedInput()
        {
            var generator = new SuccessBooleanResponseModifier();
            generator.WrapResourceObject = true;
            TestGeneratorForResource(generator, true);
        }

        [Fact]
        public void SuccessBoolean_ResourceResponse_HasSameAsInput()
        {
            var generator = new SuccessBooleanResponseModifier();
            generator.WrapResourceObject = false;
            TestGeneratorForResource(generator, false);
        }

        [Fact]
        public void StatusCode_ResourceResponse_HasWrappedInput()
        {
            var generator = new StatusCodeResponseModifier();
            generator.WrapResourceObject = true;
            TestGeneratorForResource(generator, true);
        }

        [Fact]
        public void StatusCode_ResourceResponse_HasSameAsInput()
        {
            var generator = new StatusCodeResponseModifier();
            generator.WrapResourceObject = false;
            TestGeneratorForResource(generator, false);
        }

        private void TestGeneratorForResource(IResponseModifier modifier, bool wrapsObject)
        {
            var aggBuilder = new AggregateResponseModifier(new MainBodyResponseModifier(), modifier);

            var response = new Response(null);

            RestItemData itemData = CreateAndAddResponseContent(aggBuilder, response);

            var generatedItemData = new RestItemData(response.GetFullBody());

            if (wrapsObject)
                generatedItemData = new RestItemData(generatedItemData["resource"]);

            Assert.Equal(itemData, generatedItemData, new ItemDataComparer());
        }

        private RestItemData CreateAndAddResponseContent(IResponseModifier modifier, Response response)
        {
            var itemData = new RestItemData();
            _fixture.AddManyTo(itemData, 10);
            var resourceBody = new ItemBody(itemData);
            modifier.AddResource(response, resourceBody);
            return itemData;
        }

        private class ItemDataComparer : IEqualityComparer<RestItemData>
        {
            public bool Equals(RestItemData x, RestItemData y)
            {
                foreach (var yPair in y)
                {
                    if (!x.ContainsKey(yPair.Key) || x[yPair.Key] != yPair.Value)
                        return false;
                }

                foreach (var xPair in x)
                {
                    if (!y.ContainsKey(xPair.Key) || y[xPair.Key] != xPair.Value)
                        return false;
                }

                return true;
            }

            public int GetHashCode(RestItemData obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
