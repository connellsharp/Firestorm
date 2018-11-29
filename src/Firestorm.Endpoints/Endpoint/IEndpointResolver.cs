using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    public interface IEndpointResolver
    {
        IRestEndpoint GetFromResource(IEndpointContext endpointContext, [NotNull] IRestResource resource);
    }
}