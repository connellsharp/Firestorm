using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Stems.Roots;
using Firestorm.Stems.Roots.Combined;
using Xunit;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Testing.Models;
using Moq;

namespace Firestorm.Stems.Tests.Roots
{
    public class DataSourceVaseStartInfoFactoryTests
    {
        private readonly DataSourceVaseStartInfoFactory _sut;

        public DataSourceVaseStartInfoFactoryTests()
        {
            var dataSourceMock = new Mock<IDataSource>();
            
            dataSourceMock
                .Setup(d => d.CreateContext<TestStem>())
                .Returns(new DataContext<TestStem>());

            var typeGetter = new ManualTypeGetter(typeof(TestStem));

            _sut = new DataSourceVaseStartInfoFactory(dataSourceMock.Object, typeGetter,
                DataSourceRootAttributeBehavior.UseAllStemsExceptDisallowed);
        }

        [Fact]
        public async Task GetStartResource_MockRootFactory_CallsGetStartResource()
        {
            var services = new TestStemsServices();
            
            _sut.GetStemTypes(services);

            IRootStartInfo startInfo = _sut.Get(services, "TestString");

            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);
            var info = await startDirectory.GetInfoAsync();

            Assert.Single(info.Resources);
            Assert.Equal("Test", info.Resources.Single().Name);
        }

        [Fact]
        public void GetChild_MockRootFactory_CallsGetStartResource()
        {
            var stemConfig = new TestStemsServices();
            
            _sut.GetStemTypes(stemConfig);

            var startResource = _sut.Get(stemConfig, "TestString");

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