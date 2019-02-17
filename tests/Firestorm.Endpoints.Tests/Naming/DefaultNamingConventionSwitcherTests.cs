using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.Formatting.Naming.Conventions;
using Xunit;

namespace Firestorm.Endpoints.Tests.Naming
{
    public class DefaultNamingConventionSwitcherTests
    {
        [Fact]
        public void GivenDefaultConfig_Construct_CodedCasePascal()
        {
            var config = new NamingConventionConfiguration();
            
            var switcher = new DefaultNamingConventionSwitcher(config);
            
            Assert.IsType<PascalCaseConvention>(switcher.CodedCase);
        }
        
        [Fact]
        public void GivenTwoLetterAcronym_Construct_CodedCasePascal()
        {
            var config = new NamingConventionConfiguration
            {
                TwoLetterAcronyms = new[] {"ID"}
            };
            
            var switcher = new DefaultNamingConventionSwitcher(config);

            string coverted = switcher.ConvertRequestedToCoded("artist_id");
            
            Assert.Equal("ArtistID", coverted);
        }
    }
}