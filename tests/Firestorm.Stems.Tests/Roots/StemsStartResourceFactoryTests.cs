using Firestorm.Stems.Roots.Combined;
using Moq;
using Xunit;

namespace Firestorm.Stems.Tests.Roots
{
    public class StemsStartResourceFactoryTests
    {
        [Fact]
        public void GetStartResource_GetChildResourceByName_PassesNameToStartInfoFactory()
        {
            var factoryMock = new Mock<IRootStartInfoFactory>();
            var services = new TestStemsServices();

            var factory = new StemsStartResourceFactory(services, factoryMock.Object);

            var context = new TestRequestContext();

            var startResource = (IRestDirectory)factory.GetStartResource(context);
            var testResource = startResource.GetChild("TestName");

            factoryMock.Verify(f => f.Get(services, "TestName"));
        }
    }
}