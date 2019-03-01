using System;

namespace Firestorm
{
    internal interface IServiceFactory
    {
        object Get(IServiceProvider serviceProvider);
    }
}