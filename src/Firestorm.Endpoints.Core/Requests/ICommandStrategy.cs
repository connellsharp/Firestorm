using System.Threading.Tasks;
using Firestorm.Rest.Web;

namespace Firestorm.Endpoints.Requests
{
    public interface ICommandStrategy<in TResource>
        where TResource : IRestResource
    {
        Task<Feedback> ExecuteAsync(TResource resource, IEndpointContext context, ResourceBody body);
    }
}