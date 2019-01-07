using System.Threading.Tasks;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Host
{
    public interface IRequestInvoker
    {
        void Initialize();
        
        Task InvokeAsync(IHttpRequestReader reader, IHttpRequestResponder responder, IRequestContext context);
    }
}