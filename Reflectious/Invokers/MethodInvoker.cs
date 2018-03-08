using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm
{
    public class MethodInvoker<TInstance, TReturn>
    {
        protected readonly TInstance Instance;
        protected readonly IMethodFinder MethodFinder;

        internal MethodInvoker(TInstance instance, [NotNull] IMethodFinder methodFinder)
        {
            Instance = instance;
            MethodFinder = methodFinder ?? throw new ArgumentNullException(nameof(methodFinder));
        }

        [PublicAPI]
        public TReturn Invoke(params object[] args)
        {
            MethodInfo method = MethodFinder.Find();
            object value = method.Invoke(Instance, args);
            return (TReturn) value;
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> MakeGeneric(params Type[] types)
        {
            MethodFinder.GenericArguments = types;
            return this; //new MethodInvoker<TInstance>(Instance, MethodFinder);
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> MakeGeneric<T1>()
        {
            return MakeGeneric(typeof(T1));
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> MakeGeneric<T1, T2>()
        {
            return MakeGeneric(typeof(T1), typeof(T2));
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> MakeGeneric<T1, T2, T3>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3));
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> MakeGeneric<T1, T2, T3, T4>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> MakeGeneric<T1, T2, T3, T4, T5>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> MakeGeneric<T1, T2, T3, T4, T5, T6>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> MakeGeneric(IEnumerable<Type> types)
        {
            return MakeGeneric(types.ToArray());
        }

        public StrongMethodInvoker<TInstance, TReturn, TParam1, TParam2> WithParameters<TParam1, TParam2>()
        {
            MethodFinder.ParameterTypes = new[] {typeof(TParam1), typeof(TParam2)};
            return new StrongMethodInvoker<TInstance, TReturn, TParam1, TParam2>(Instance, MethodFinder);
        }
    }

    public class MethodInvoker<TInstance> : MethodInvoker<TInstance, object>
    {
        public MethodInvoker(TInstance instance, [NotNull] MethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> ReturnsType<TReturn>()
        {
            return new MethodInvoker<TInstance, TReturn>(Instance, MethodFinder);
        }
    }

    public class MethodInvoker : MethodInvoker<object>
    {
        public MethodInvoker(object instance, [NotNull] MethodFinder methodFinder)
            : base(instance, methodFinder)
        {
        }
    }
}