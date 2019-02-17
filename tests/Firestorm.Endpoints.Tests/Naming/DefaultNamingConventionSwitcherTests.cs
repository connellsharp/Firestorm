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
            
            var calculator = new DefaultNamingConventionSwitcher(config);
            
            Assert.IsType<PascalCaseConvention>(calculator.CodedCase);
        }
    }
}