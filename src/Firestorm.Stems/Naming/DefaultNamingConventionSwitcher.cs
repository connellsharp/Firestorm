namespace Firestorm.Stems.Naming
{
    public class DefaultNamingConventionSwitcher : NamingConventionSwitcher
    {
        public DefaultNamingConventionSwitcher()
            : base(
                codedCase: new PascalCaseConvention(),
                defaultOutputCase: new SnakeCaseConvention(),
                allowedCases: new ICaseConvention[] { new SnakeCaseConvention(), new PascalCaseConvention(), new CamelCaseConvention(), new KebabCaseConvention() }
            )
        { }
    }
}