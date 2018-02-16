namespace Firestorm.Endpoints.Web
{
    public interface IHttpRequestHandler : IHttpRequestResponder, IHttpRequestReader
    {
        // This has been split into the two interfaces.
        // TODO implement the two interfaces separately in the Web libraries.
    }
}