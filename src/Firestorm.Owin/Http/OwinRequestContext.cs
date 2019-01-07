using System;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;
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