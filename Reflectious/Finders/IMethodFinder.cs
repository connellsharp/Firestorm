using System;
using System.Reflection;

namespace Firestorm
{
    public interface IMethodFinder
    {
        Type[] GenericArguments { set; }
        Type[] ParameterTypes { set; }
        
        MethodInfo FindMethodInfo();
        object FindAndInvoke(object instance, object[] args);
    }
}