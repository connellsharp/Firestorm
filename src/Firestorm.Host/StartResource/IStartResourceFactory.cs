using Firestorm.Host.Infrastructure;

namespace Firestorm.Host
{
    /// <summary>
    /// Factory interface defines how to get the starting resource (i.e. first directory) of a REST API.
    /// </summary>
    public interface IStartResourceFactory
    {
        void Initialize();
        
        IRestResource GetStartResource(IRequestContext requestContext);
    }
}