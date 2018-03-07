using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm
{
    public class MethodInvoker
    {
        private readonly object _instance;
        private readonly MethodInfo _method;

        public MethodInvoker(object instance, [NotNull] MethodInfo method)
        {
            _instance = instance;
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public MethodInvoker MakeGeneric<T>()
        {
            return MakeGeneric(typeof(T));
        }

        public MethodInvoker MakeGeneric<T1, T2>()
        {
            return MakeGeneric(typeof(T1), typeof(T2));
        }

        public MethodInvoker MakeGeneric<T1, T2, T3>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3));
        }

        public MethodInvoker MakeGeneric<T1, T2, T3, T4>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public MethodInvoker MakeGeneric<T1, T2, T3, T4, T5>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public MethodInvoker MakeGeneric<T1, T2, T3, T4, T5, T6>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        }

        public MethodInvoker MakeGeneric(params Type[] types)
        {
            var genericMethod = _method.MakeGenericMethod(types);
            return new MethodInvoker(_instance, genericMethod);
        }
        
        public MethodInvoker MakeGeneric(IEnumerable<Type> types)
        {
            return MakeGeneric(types.ToArray());
        }
        
        public object Invoke(params object[] args)
        {
            return _method.Invoke(_instance, args);
        }
    }
}