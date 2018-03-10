using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firestorm
{
    internal static class MatchUtilities
    {
        internal static bool MatchesParameterCount(MethodBase method, Type[] parameterTypes)
        {
            ParameterInfo[] ctorParams = method.GetParameters();
            return ctorParams.Length == parameterTypes.Length;
        }
        
        internal static bool MatchesParameterTypes(MethodBase method, Type[] parameterTypes)
        {
            ParameterInfo[] ctorParams = method.GetParameters();

            if (ctorParams.Length != parameterTypes.Length)
                return false;

            return MatchesTypes(ctorParams.Select(p => p.ParameterType), parameterTypes);
        }

        public static bool MatchesGenericArgumentCount(MethodBase method, Type[] genericArguments)
        {
            Type[] genericArgs = method.GetGenericArguments();
            return genericArgs.Length == genericArguments.Length;
        }

        public static bool MatchesGenericArgumentTypes(MethodBase method, Type[] genericArguments)
        {
            var genericArgs = method.GetGenericArguments();

            if (genericArgs.Length != genericArguments.Length)
                return false;
            
            return MatchesTypes(genericArgs, genericArguments);
        }

        private static bool MatchesTypes(IEnumerable<Type> types, IEnumerable<Type> assignableTypes)
        {
            using (var enumerator = types.GetEnumerator())
            {
                foreach (var assignableType in assignableTypes)
                {
                    if (!enumerator.MoveNext())
                        return false;

                    if (!enumerator.Current.IsAssignableFrom(assignableType))
                        return false;
                }

                if (enumerator.MoveNext())
                    return false;
            }

            return true;
        }
    }
}