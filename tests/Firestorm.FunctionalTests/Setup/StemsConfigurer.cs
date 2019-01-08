using Firestorm.Stems;
using Xunit;

namespace Firestorm.FunctionalTests.Setup
{
    [CollectionDefinition(nameof(StemsCollectionDefinition))]
    public class StemsCollectionDefinition : ICollectionFixture<StemsTestFixture>
    {
        // xunit collection fixture class
    }
    
    public class StemsTestFixture : FunctionalTestFixture<StemsConfigurer>
    {
    }
    
    public class StemsConfigurer : IStartupConfigurer
    {
        public void Configure(IFirestormServicesBuilder builder)
        {
            builder.AddStems();
        }
    }
}