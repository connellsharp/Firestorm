using System;
using System.Reflection;

namespace Firestorm
{
    public class SingleMethodFinder : MemberFinder, IMethodFinder
    {
        public SingleMethodFinder(Type type, string methodName, bool isStatic) 
            : base(type, methodName, isStatic)
        {
        }

        public Type[] GenericArguments { get; set; }
        public Type[] ParameterTypes { get; set; }

        public MethodInfo FindMethodInfo()
        {
            MethodInfo method = Type.GetMethod(MemberName, GetBindingFlags());
            return method;
        }

        public object FindAndInvoke(object instance, object[] args)
        {
            MethodInfo method = FindMethodInfo();
            return method.Invoke(instance, args);
        }
    }
}