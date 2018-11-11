using System;
using Firestorm.Endpoints;
using Firestorm.Host;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    public class OwinRequestContext : IRequestContext
    {
        public OwinRequestContext(IOwinContext owinContext)
        {
            User = new PrincipalUser(owinContext.Request.User);
        }

        public IRestUser User { get; }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}