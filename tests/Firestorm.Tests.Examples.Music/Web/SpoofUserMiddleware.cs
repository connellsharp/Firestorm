using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Firestorm.Tests.Examples.Music.Web
{
    public class SpoofUserMiddleware : OwinMiddleware
    {
        public SpoofUserMiddleware(OwinMiddleware next) 
            : base(next)
        { }

        public override Task Invoke(IOwinContext context)
        {
            context.Request.User = new GenericPrincipal(new GenericIdentity("Me"), new string[] { });
            return Next.Invoke(context);
        }
    }
}