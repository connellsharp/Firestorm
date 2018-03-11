using System.Reflection;

namespace Firestorm
{
    public class ReflectionMethod : IMethod
    {
        private readonly MethodInfo _methodInfo;

        public ReflectionMethod(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }

        public object Invoke(object instance, object[] args)
        {
            return _methodInfo.Invoke(instance, args);
        }

        public MethodInfo GetMethodInfo()
        {
            return _methodInfo;
        }
    }
}