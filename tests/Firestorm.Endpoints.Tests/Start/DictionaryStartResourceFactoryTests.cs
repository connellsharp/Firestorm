using System.Threading.Tasks;
using Firestorm.Endpoints.Web;
using Xunit;

namespace Firestorm.Endpoints.Tests.Start
{
    public class DictionaryStartResourceFactoryTests
    {
        public class FakeResource : IRestScalar
        {
            public async Task<object> GetAsync()
            {
                return "yay";
            }

            public async Task<Acknowledgment> EditAsync(object value)
            {
                return new Acknowledgment();
            }
        }

        [Fact]
        public void GetStartResourceTest()
        {
            var factory = new DictionaryDirectory
            {
                { "fake", typeof(FakeResource) }
            };

            var resource = factory.GetChild("fake");
            Assert.IsType<FakeResource>(resource);
        }
    }
}