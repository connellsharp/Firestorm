using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firestorm.Stems.Roots.Combined;
using Firestorm.Stems.Roots.DataSource;
using FluentAssertions;
using Moq;
using Xunit;

namespace Firestorm.Stems.Tests.Roots
{
    public class StemsStartResourceFactoryTests
    {
        [Fact]
        public void GetStartResource_ReturnsDirectory()
        {
            var services = new TestStemsServices();
            
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            
            var factoryMock = fixture.Freeze<Mock<IRootStartInfoFactory>>();
            
            factoryMock.Setup(f => f.GetStemTypes(services)).Returns(new[] {typeof(TestStem)});

            var factory = new StemsStartResourceFactory(services, factoryMock.Object);

            var context = new TestRequestContext();

            var startResource = factory.GetStartResource(context);

            startResource.Should().BeAssignableTo<IRestDirectory>();
        }
    }
}