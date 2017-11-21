namespace Firestorm.Fluent.Sources
{
    public class SourceCreator
    {
        public IApiDirectorySource CreateSource(ApiContext apiContext, IApiBuilder builder)
        {
            apiContext.OnModelCreating(builder);
            return builder.BuildSource();
        }
    }
}
