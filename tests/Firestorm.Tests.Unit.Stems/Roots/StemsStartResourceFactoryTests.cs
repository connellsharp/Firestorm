using Firestorm.Stems.Roots;
using Moq;
using Xunit;

namespace Firestorm.Tests.Unit.Stems.Roots
{
    public class StemsStartResourceFactoryTests
    {
        [Fact]
        public void GetStartResource_MockRootFactory_CallsGetStartResource()
        {
            var rootFactoryMock = new Mock<IRootResourceFactory>();
            var stemConfig = new DefaultStemConfiguration();

            var factory = new StemsStartResourceFactory
            {
                StemConfiguration = stemConfig,
                RootResourceFactory = rootFactoryMock.Object
            };

            var context = new TestEndpointContext();

            var startResource = factory.GetStartResource(context);

            rootFactoryMock.Verify(f => f.GetStartResource(It.IsAny<EndpointRootRequest>(), stemConfig));
        }
    }
}