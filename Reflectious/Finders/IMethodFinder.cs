using System;

namespace Firestorm
{
    internal interface IMethodFinder
    {
        Type[] GenericArguments { set; }
        Type[] ParameterTypes { set; }
        bool WantsParameterTypes { get; }

        string GetCacheKey();
        IMethod Find();
    }
}