using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Firestorm.Stems.FunctionalTests.Web
{
    public class SpoofUserMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.User = new GenericPrincipal(new GenericIdentity("Me"), new string[] { });
            return next.Invoke(context);
        }
    }
}