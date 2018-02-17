namespace Firestorm.Endpoints.Naming
{
    public class DefaultNamingConventionSwitcher : NamingConventionSwitcher
    {
        public DefaultNamingConventionSwitcher(NamingConventionOptions options)
            : base(
                codedCase: new PascalCaseConvention(options.TwoLetterAcronyms),
                defaultOutputCase: new SnakeCaseConvention(),
                allowedCases: new ICaseConvention[] { new SnakeCaseConvention(), new PascalCaseConvention(options.TwoLetterAcronyms), new CamelCaseConvention(options.TwoLetterAcronyms), new KebabCaseConvention() }
            )
        {
            
        }

        public DefaultNamingConventionSwitcher()
            : this(new NamingConventionOptions())
        { }
    }
}