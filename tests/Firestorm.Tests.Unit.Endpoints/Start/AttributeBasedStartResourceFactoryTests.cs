using System;
using System.Threading.Tasks;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Start;
using Xunit;

namespace Firestorm.Tests.Endpoints.Start
{
    public class AttributeBasedStartResourceFactoryTests
    {
        [Fact]
        public async Task Writer_AttributeBasedStartResourceFactory_DoesntThrow()
        {
            var factory = new AttributeBasedDirectory();
            factory.LoadFromAttributes();
            var resource = factory.GetChild("test");
            var scalar = resource as IRestScalar;
            Assert.NotNull(scalar);
            var value = await scalar.GetAsync();
            Assert.Equal("Blahblah",value);
        }
    }

    [RestStartResource("test")]
    public class TestScalar : IRestScalar
    {
        public async Task<object> GetAsync()
        {
            return "Blahblah";
        }

        public Task<Acknowledgment> EditAsync(object value)
        {
            throw new NotImplementedException();
        }
    }
}