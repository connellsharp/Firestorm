using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Xunit;
using Firestorm.Stems.Roots.DataSource;
using Moq;

namespace Firestorm.Tests.Unit.Stems.Roots
{
    public class DataSourceResourceFactoryTests
    {
        private readonly DataSourceRootResourceFactory _factory;

        public DataSourceResourceFactoryTests()
        {
            var dataSourceMock = new Mock<IDataSource>();

            _factory = new DataSourceRootResourceFactory
            {
                StemTypeGetter = new ManualTypeGetter(typeof(TestStem)),
                DataSource = dataSourceMock.Object
            };
        }

        [Fact]
        public async Task GetStartResource_MockRootFactory_CallsGetStartResource()
        {
            _factory.GetStemTypes();

            var startResource = _factory.GetStartResource(new TestRootRequest(), new DefaultStemConfiguration());

            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);
            var info = await startDirectory.GetInfoAsync();

            Assert.Equal(1, info.Resources.Count());
            Assert.Equal("Test", info.Resources.Single().Name);
        }

        [Fact]
        public void GetChild_MockRootFactory_CallsGetStartResource()
        {
            _factory.GetStemTypes();

            var startResource = _factory.GetStartResource(new TestRootRequest(), new DefaultStemConfiguration());

            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);
            var resource = startDirectory.GetChild("Test");
            var collection = Assert.IsAssignableFrom<IRestCollection>(resource);

        }

        [DataSourceRoot]
        public class TestStem : Stem<Artist>
        {
            public override bool CanAddItem()
            {
                return true;
            }
        }
    }
}