using System.Threading.Tasks;
using Firestorm.Endpoints.Web;

namespace Firestorm.Host
{
    public interface IRequestInvoker
    {
        void Initialize();
        
        Task InvokeAsync(IHttpRequestReader reader, IHttpRequestResponder responder, IRequestContext context);
    }
}