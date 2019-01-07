using System.Threading.Tasks;
using Firestorm.Host;
using Firestorm.Owin.Http;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    [UsedImplicitly]
    public class FirestormMiddleware : OwinMiddleware
    {
        private readonly IRequestInvoker _invoker;

        public FirestormMiddleware(OwinMiddleware next, IRequestInvoker invoker)
            : base(next)
        {
            _invoker = invoker;

            _invoker.Initialize(); // TODO check lifetime of this
        }

        public override async Task Invoke(IOwinContext owinContext)
        {            
            var reader = new OwinContextReader(owinContext);
            var responder = new OwinContextResponder(owinContext);
            var context = new OwinRequestContext(owinContext);

            await _invoker.InvokeAsync(reader, responder, context);

            //await Next.Invoke(owinContext);
        }
    }
}