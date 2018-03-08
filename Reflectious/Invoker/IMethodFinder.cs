using System;
using System.Reflection;

namespace Firestorm
{
    public interface IMethodFinder
    {
        MethodInfo Find();
        Type[] GenericArguments { set; }
    }
}