using System;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Testing.Http
{
    public class TestRequestContext : IRequestContext
    {

        public IRestUser User { get; } = new TestUser();

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}