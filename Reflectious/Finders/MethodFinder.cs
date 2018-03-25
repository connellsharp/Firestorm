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
        public MethodFinder(Type classType, string methodName, bool isStatic) 
            : base(classType, methodName, isStatic)
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

            var delegateCreator = new FuncDelegateCreator(IsStatic ? null : ClassType, ParameterTypes, methodInfo.ReturnType);
            Type delegateType = delegateCreator.GetDelegateType();
            Delegate del = Delegate.CreateDelegate(delegateType, methodInfo);
            
            return new DelegateMethod(del);
        }

        private MethodInfo FindMethodInfo()
        {
            IEnumerable<MethodInfo> methods = ClassType.GetMethods(GetBindingFlags())
                .Where(m => m.Name == MemberName);

            if (GenericArguments != null)
                methods = methods.Where(c => MatchUtilities.MatchesGenericArgumentCount(c, GenericArguments));

            if (ParameterTypes != null)
                methods = methods.Where(c => MatchUtilities.MatchesParameterCount(c, ParameterTypes));

            var methodsList = methods.ToList();

            if (methodsList.Count == 0)
                throw new MethodNotFoundException(MemberName);

            MethodInfo method = methodsList.Single();

            if (method.IsGenericMethodDefinition)
            {
                if (GenericArguments == null)
                    GenericArguments = InferGenericArguments(method);
                
                method = method.MakeGenericMethod(GenericArguments);
            }

            return method;
        }

        private Type[] InferGenericArguments(MethodInfo method)
        {
            var genericParams = method.GetGenericArguments();
            if(genericParams.Length != 1)
                throw new NotImplementedException("Currently only implemented infering generic arguments for methods with one generic parameter.");
            
            ParameterInfo[] parameters = method.GetParameters();
            
            if(parameters.Length != ParameterTypes.Length)
                throw new InvalidOperationException("Incorrect number of parameters.");
            
            if (!EnumerableTypeUtility.IsEnumerable(ParameterTypes[0]))
                throw new NotImplementedException("Currently only implemented infering generic arguments when the first parameter is an enumerable type.");

            return new[] { EnumerableTypeUtility.GetItemType(ParameterTypes[0]) };
        }
    }
}