using System;
using System.Collections.Generic;
using System.Linq;
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
        public Type[] ParameterTypes { get; set; }

        public MethodInfo FindMethodInfo()
        {
            //MethodInfo method = Type.GetMethod(MemberName, GetBindingFlags());

            IEnumerable<MethodInfo> methods = Type.GetMethods(GetBindingFlags())
                .Where(m => m.Name == MemberName);

            if (GenericArguments != null)
                methods = methods.Where(m => m.GetGenericArguments().Length == GenericArguments.Length);

            if (ParameterTypes != null)
                methods = methods.Where(m => m.GetParameters().Length == ParameterTypes.Length); // TODO check types?

            var methodsList = methods.ToList();
            
            if (methodsList.Count == 0)
                throw new MethodNotFoundException(MemberName);

            MethodInfo method = methodsList.Single();

            if (GenericArguments != null)
                method = method.MakeGenericMethod(GenericArguments);

            return method;
        }

        public object FindAndInvoke(object instance, object[] args)
        {
            var method = FindMethodInfo();
            return method.Invoke(instance, args);
        }
    }
}