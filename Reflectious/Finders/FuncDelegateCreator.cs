using System;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm
{
    internal class FuncDelegateCreator
    {
        private readonly Type _instanceType;
        private readonly Type[] _parameterTypes;
        private readonly Type _returnType;

        public FuncDelegateCreator(Type instanceType, Type[] parameterTypes, Type returnType)
        {
            _instanceType = instanceType;
            _parameterTypes = parameterTypes;
            _returnType = returnType;
        }

        public Type GetDelegateType()
        {
            Type[] types = GetFuncGenericArgs().ToArray();
            return GetFuncType(types.Length).MakeGenericType(types);
        }

        private IEnumerable<Type> GetFuncGenericArgs()
        {
            // open instance delegates have the first type as the instance type
            if (_instanceType != null)
                yield return _instanceType;

            if (_parameterTypes != null)
                foreach (var parameterType in _parameterTypes)
                    yield return parameterType;

            yield return _returnType;
        }

        private static Type GetFuncType(int parameterCount)
        {
            switch (parameterCount)
            {
                case 1:
                    return typeof(Func<>);
                case 2:
                    return typeof(Func<,>);
                case 3:
                    return typeof(Func<,,>);
                case 4:
                    return typeof(Func<,,,>);
                case 5:
                    return typeof(Func<,,,,>);
                case 6:
                    return typeof(Func<,,,,,>);
                case 7:
                    return typeof(Func<,,,,,,>);
                case 8:
                    return typeof(Func<,,,,,,,>);
                default:
                    throw new NotSupportedException("More than 6 parameters are not supported.");
            }
        }
    }
}