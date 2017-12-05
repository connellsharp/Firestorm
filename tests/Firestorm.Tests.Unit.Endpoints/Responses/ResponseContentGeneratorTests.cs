﻿using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Responses;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Responses
{
    public class ResponseContentGeneratorTests
    {
        private readonly Fixture _fixture;

        public ResponseContentGeneratorTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [Fact]
        public void SuccessBoolean_ResourceResponse_HasWrappedInput()
        {
            var generator = new SuccessBooleanResponseContentGenerator();
            generator.WrapResourceObject = true;
            TestGeneratorForResource(generator, true);
        }

        [Fact]
        public void StatusCode_ResourceResponse_HasWrappedInput()
        {
            var generator = new StatusCodeResponseContentGenerator();
            generator.WrapResourceObject = true;
            TestGeneratorForResource(generator, true);
        }

        [Fact]
        public void StatusCode_ResourceResponse_HasSameAsInput()
        {
            var generator = new StatusCodeResponseContentGenerator();
            generator.WrapResourceObject = false;
            TestGeneratorForResource(generator, false);
        }

        private void TestGeneratorForResource(IResponseContentGenerator generator, bool wrapsObject)
        {
            var itemData = new RestItemData();
            _fixture.AddManyTo(itemData, 10);

            var result = generator.GetFromResource(new ItemBody(itemData));

            var generatedItemData = new RestItemData(result);

            if (wrapsObject)
                generatedItemData = new RestItemData(generatedItemData["resource"]);

            Assert.Equal(itemData, generatedItemData, new ItemDataComparer());
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