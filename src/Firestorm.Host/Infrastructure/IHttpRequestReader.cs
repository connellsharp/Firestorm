namespace Firestorm.Host.Infrastructure
{
    public interface IHttpRequestReader
    {
        string RequestMethod { get; }

        string ResourcePath { get; }

        IPreconditions GetPreconditions();
        
        IContentReader GetContentReader();

        string GetQueryString();
    }
}