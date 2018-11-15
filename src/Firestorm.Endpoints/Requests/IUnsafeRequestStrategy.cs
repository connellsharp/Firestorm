using System.Threading.Tasks;
using Firestorm.Core.Web;

namespace Firestorm.Endpoints.Strategies
{
    public interface IUnsafeRequestStrategy<in TResource>
        where TResource : IRestResource
    {
        Task<Feedback> ExecuteAsync(TResource resource, IEndpointContext context, ResourceBody body);
    }
}