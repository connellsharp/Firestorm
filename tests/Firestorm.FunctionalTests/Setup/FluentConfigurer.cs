using Firestorm.Fluent;
using Firestorm.FunctionalTests.Web;
using Xunit;

namespace Firestorm.FunctionalTests.Setup
{
    [CollectionDefinition(nameof(FluentCollectionDefinition))]
    public class FluentCollectionDefinition : ICollectionFixture<FluentTestFixture>
    {
        // xunit collection fixture class
    }
    
    public class FluentTestFixture : FunctionalTestFixture<FluentConfigurer>
    {
    }
    
    public class FluentConfigurer : IStartupConfigurer
    {
        public void Configure(IServicesBuilder builder)
        {
            builder.AddFluent<FootballApiContext>();
        }
    }
}