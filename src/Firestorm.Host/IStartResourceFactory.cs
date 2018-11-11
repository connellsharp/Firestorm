namespace Firestorm.Host
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