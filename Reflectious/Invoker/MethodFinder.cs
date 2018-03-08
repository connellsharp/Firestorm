using System;
using System.Reflection;

namespace Firestorm
{
    public class MethodFinder : MemberFinder, IMethodFinder
    {
        public MethodFinder(Type type, string methodName, bool isStatic) 
            : base(type, methodName, isStatic)
        {
        }

        public Type[] GenericArguments { get; set; }

        public MethodInfo Find()
        {
            MethodInfo method = Type.GetMethod(MemberName, GetBindingFlags());

            if (method == null)
                throw new MethodNotFoundException(MemberName);

            if (GenericArguments != null)
                method = method.MakeGenericMethod(GenericArguments);

            return method;
        }
    }
}