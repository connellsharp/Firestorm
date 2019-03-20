using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Stems.Roots;
using Xunit;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Testing.Models;
using Moq;

namespace Firestorm.Stems.Tests.Roots
{
    public class DataSourceResourceFactoryTests
    {
        private readonly DataSourceRootResourceFactory _factory;

        public DataSourceResourceFactoryTests()
        {
            var dataSourceMock = new Mock<IDataSource>();
            
            dataSourceMock
                .Setup(d => d.CreateContext<TestStem>())
                .Returns(new DataContext<TestStem>());

            _factory = new DataSourceRootResourceFactory
            {
                StemTypeGetter = new ManualTypeGetter(typeof(TestStem)),
                DataSource = dataSourceMock.Object
            };
        }

        [Fact]
        public async Task GetStartResource_MockRootFactory_CallsGetStartResource()
        {
            var services = new TestStemsServices();
            
            _factory.GetStemTypes(services);

            var startResource = _factory.GetStartResource(services, new TestRequestContext());

            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);
            var info = await startDirectory.GetInfoAsync();

            Assert.Single(info.Resources);
            Assert.Equal("Test", info.Resources.Single().Name);
        }

        [Fact]
        public void GetChild_MockRootFactory_CallsGetStartResource()
        {
            var stemConfig = new TestStemsServices();
            
            _factory.GetStemTypes(stemConfig);

            var startResource = _factory.GetStartResource(stemConfig, new TestRequestContext());

            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);
            var resource = startDirectory.GetChild("Test");
            var collection = Assert.IsAssignableFrom<IRestCollection>(resource);

        }

        [DataSourceRoot]
        public class TestStem : Stem<Artist>
        {
        }
    }
}