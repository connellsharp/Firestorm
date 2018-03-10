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
                methods = methods.Where(c => MatchUtilities.MatchesGenericArgumentCount(c, GenericArguments));

            if (ParameterTypes != null)
                methods = methods.Where(c => MatchUtilities.MatchesParameterCount(c, ParameterTypes));

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
            MethodInfo method = FindMethodInfo();
            return method.Invoke(instance, args);
        }
    }
}