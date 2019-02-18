using Firestorm.Stems.Roots;
using Moq;
using Xunit;

namespace Firestorm.Stems.Tests.Roots
{
    public class StemsStartResourceFactoryTests
    {
        [Fact]
        public void GetStartResource_MockRootFactory_CallsGetStartResource()
        {
            var rootFactoryMock = new Mock<IRootResourceFactory>();
            var services = new TestStemsServices();

            var factory = new StemsStartResourceFactory
            {
                StemsServices = services,
                RootResourceFactory = rootFactoryMock.Object
            };

            var context = new TestRequestContext();

            var startResource = factory.GetStartResource(context);

            rootFactoryMock.Verify(f => f.GetStartResource(services, context));
        }
    }
}