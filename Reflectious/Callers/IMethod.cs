using System.Reflection;

namespace Firestorm
{
    public interface IMethod
    {
        object Invoke(object instance, object[] args);
        
        MethodInfo GetMethodInfo();
    }
}