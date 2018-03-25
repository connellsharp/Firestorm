using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm
{
    public class WeakMethodReflector<TInstance, TReturn> : MethodReflectorBase<TInstance, TReturn>
    {
        internal WeakMethodReflector(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public new TReturn Invoke(params object[] args)
        {
            return base.Invoke(args);
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> MakeGeneric(params Type[] types)
        {
            MethodFinder.GenericArguments = types;
            return this; //new MethodInvoker<TInstance>(Instance, MethodFinder);
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> MakeGeneric<T1>()
        {
            return MakeGeneric(typeof(T1));
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> MakeGeneric<T1, T2>()
        {
            return MakeGeneric(typeof(T1), typeof(T2));
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> MakeGeneric<T1, T2, T3>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3));
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> MakeGeneric<T1, T2, T3, T4>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> MakeGeneric<T1, T2, T3, T4, T5>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> MakeGeneric<T1, T2, T3, T4, T5, T6>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> MakeGeneric(IEnumerable<Type> types)
        {
            return MakeGeneric(types.ToArray());
        }

        [PublicAPI]
        public StrongMethodReflector<TInstance, TReturn, TParam1> WithParameters<TParam1>()
        {
            WithParameters(typeof(TParam1));
            return new StrongMethodReflector<TInstance, TReturn, TParam1>(Instance, MethodFinder);
        }

        [PublicAPI]
        public StrongMethodReflector<TInstance, TReturn, TParam1, TParam2> WithParameters<TParam1, TParam2>()
        {
            WithParameters(typeof(TParam1), typeof(TParam2));
            return new StrongMethodReflector<TInstance, TReturn, TParam1, TParam2>(Instance, MethodFinder);
        }

        [PublicAPI]
        public StrongMethodReflector<TInstance, TReturn, TParam1, TParam2, TParam3> WithParameters<TParam1, TParam2, TParam3>()
        {
            WithParameters(typeof(TParam1), typeof(TParam2), typeof(TParam3));
            return new StrongMethodReflector<TInstance, TReturn, TParam1, TParam2, TParam3>(Instance, MethodFinder);
        }

        [PublicAPI]
        public StrongMethodReflector<TInstance, TReturn, TParam1, TParam2, TParam3, TParam4> WithParameters<TParam1, TParam2, TParam3, TParam4>()
        {
            WithParameters(typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4));
            return new StrongMethodReflector<TInstance, TReturn, TParam1, TParam2, TParam3, TParam4>(Instance, MethodFinder);
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> WithParameters(params Type[] parameterTypes)
        {
            MethodFinder.ParameterTypes = parameterTypes;
            return this;
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> WithParameters(IEnumerable<Type> parameterTypes)
        {
            return WithParameters(parameterTypes.ToArray());
        }
    }

    public class WeakMethodReflector<TInstance> : WeakMethodReflector<TInstance, object>
    {
        internal WeakMethodReflector(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public WeakMethodReflector<TInstance, TReturn> ReturnsType<TReturn>()
        {
            return new WeakMethodReflector<TInstance, TReturn>(Instance, MethodFinder);
        }
    }

    public class WeakMethodReflector : WeakMethodReflector<object>
    {
        internal WeakMethodReflector(object instance, [NotNull] IMethodFinder methodFinder)
            : base(instance, methodFinder)
        {
        }
    }
}