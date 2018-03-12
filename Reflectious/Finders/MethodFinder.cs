using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Firestorm
{
    internal class MethodFinder : MemberFinder, ICacheableMethodFinder
    {
        public MethodFinder(Type type, string methodName, bool isStatic) 
            : base(type, methodName, isStatic)
        {
        }

        public Type[] GenericArguments { get; set; }
        public Type[] ParameterTypes { get; set; }
        public bool WantsParameterTypes => ParameterTypes == null;

        public string GetCacheKey()
        {
            var builder = new StringBuilder(MemberName);
            builder.AppendFullTypeNames(GenericArguments);
            builder.AppendFullTypeNames(ParameterTypes);
            return builder.ToString();
        }

        public IMethod Find()
        {
            MethodInfo methodInfo = FindMethodInfo();

            var delegateCreator = new FuncDelegateCreator(IsStatic ? null : Type, ParameterTypes, methodInfo.ReturnType);
            Type delegateType = delegateCreator.GetDelegateType();
            Delegate del = Delegate.CreateDelegate(delegateType, methodInfo);
            
            return new DelegateMethod(del);
        }

        private MethodInfo FindMethodInfo()
        {
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
    }
}