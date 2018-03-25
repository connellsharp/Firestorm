using System;

namespace Firestorm
{
    internal interface IMethodFinder
    {
        Type[] GenericArguments { get; set; }
        Type[] ParameterTypes { get; set; }
        bool WantsParameterTypes { get; }

        IMethod Find();
    }

    internal interface ICacheableMethodFinder : IMethodFinder
    {
        string GetCacheKey();
    }
}