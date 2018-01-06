namespace Firestorm.Fluent.Sources
{
    public class SourceCreator
    {
        public IApiDirectorySource CreateSource(RestContext restContext, IApiBuilder builder)
        {
            restContext.OnApiCreating(builder);
            return builder.BuildSource();
        }
    }
}
