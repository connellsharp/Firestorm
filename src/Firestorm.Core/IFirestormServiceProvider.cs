using System;

namespace Firestorm
{
    public interface IFirestormServiceProvider : IServiceProvider
    {
        IServiceProvider GetRequestServiceProvider();
    }
}