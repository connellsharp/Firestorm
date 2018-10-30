using Firestorm.Host;

namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// Factory interface defines how to get the starting resource (i.e. first directory) of a REST API.
    /// </summary>
    public interface IStartResourceFactory
    {
        IRestResource GetStartResource(IRequestContext requestContext);

        void Initialize();
    }
}