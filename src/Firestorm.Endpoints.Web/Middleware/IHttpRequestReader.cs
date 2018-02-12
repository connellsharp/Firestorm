using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Preconditions;

namespace Firestorm.Endpoints.Start
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