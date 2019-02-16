using Firestorm.Endpoints.Formatting.Naming.Conventions;

namespace Firestorm.Endpoints.Formatting.Naming
{
    public class DefaultNamingConventionSwitcher : NamingConventionSwitcher
    {
        public DefaultNamingConventionSwitcher(NamingConventionConfiguration configuration)
            : base(
                codedCase: new PascalCaseConvention(configuration.TwoLetterAcronyms),
                defaultOutputCase: new SnakeCaseConvention(),
                allowedCases: new ICaseConvention[] { new SnakeCaseConvention(), new PascalCaseConvention(configuration.TwoLetterAcronyms), new CamelCaseConvention(configuration.TwoLetterAcronyms), new KebabCaseConvention() }
            )
        {
            
        }
    }
}