namespace Firestorm.FunctionalTests.Setup
{
    public interface IStartupConfigurer
    {
        void Configure(IServicesBuilder builder);
    }
}