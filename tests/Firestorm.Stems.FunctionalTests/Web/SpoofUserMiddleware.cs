using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Firestorm.Stems.FunctionalTests.Web
{
    public class SpoofUserMiddleware
    {
        private readonly RequestDelegate _next;

        public SpoofUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public Task Invoke(HttpContext context)
        {
            context.User = new GenericPrincipal(new GenericIdentity("Me"), new string[] { });
            return _next.Invoke(context);
        }
    }
}