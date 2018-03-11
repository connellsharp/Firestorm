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
        internal readonly IMethodFinder MethodFinder;

        internal MethodInvoker(TInstance instance, [NotNull] IMethodFinder methodFinder)
        {
            Instance = instance;
            MethodFinder = methodFinder ?? throw new ArgumentNullException(nameof(methodFinder));
        }

        private static readonly Cache<IMethod> MethodCache = new Cache<IMethod>();
        
        private IMethod GetMethod()
        {
            return MethodCache.GetOrAdd(MethodFinder.GetCacheKey(), () => MethodFinder.Find());
        }

        [PublicAPI]
        public MethodInfo MethodInfo => GetMethod().GetMethodInfo();

        [PublicAPI]
        public TReturn Invoke(params object[] args)
        {
            if(MethodFinder.WantsParameterTypes)
                MethodFinder.ParameterTypes = args.Select(a => a.GetType()).ToArray();
            
            IMethod method = GetMethod();
            
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

        [PublicAPI]
        public StrongMethodInvoker<TInstance, TReturn, TParam1> WithParameters<TParam1>()
        {
            WithParameters(typeof(TParam1));
            return new StrongMethodInvoker<TInstance, TReturn, TParam1>(Instance, MethodFinder);
        }

        [PublicAPI]
        public StrongMethodInvoker<TInstance, TReturn, TParam1, TParam2> WithParameters<TParam1, TParam2>()
        {
            WithParameters(typeof(TParam1), typeof(TParam2));
            return new StrongMethodInvoker<TInstance, TReturn, TParam1, TParam2>(Instance, MethodFinder);
        }

        [PublicAPI]
        public StrongMethodInvoker<TInstance, TReturn, TParam1, TParam2, TParam3> WithParameters<TParam1, TParam2, TParam3>()
        {
            WithParameters(typeof(TParam1), typeof(TParam2), typeof(TParam3));
            return new StrongMethodInvoker<TInstance, TReturn, TParam1, TParam2, TParam3>(Instance, MethodFinder);
        }

        [PublicAPI]
        public StrongMethodInvoker<TInstance, TReturn, TParam1, TParam2, TParam3, TParam4> WithParameters<TParam1, TParam2, TParam3, TParam4>()
        {
            WithParameters(typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4));
            return new StrongMethodInvoker<TInstance, TReturn, TParam1, TParam2, TParam3, TParam4>(Instance, MethodFinder);
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> WithParameters(params Type[] parameterTypes)
        {
            MethodFinder.ParameterTypes = parameterTypes;
            return this;
        }

        [PublicAPI]
        public MethodInvoker<TInstance, TReturn> WithParameters(IEnumerable<Type> parameterTypes)
        {
            return WithParameters(parameterTypes.ToArray());
        }
    }

    public class MethodInvoker<TInstance> : MethodInvoker<TInstance, object>
    {
        internal MethodInvoker(TInstance instance, [NotNull] IMethodFinder methodFinder) 
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
        internal MethodInvoker(object instance, [NotNull] IMethodFinder methodFinder)
            : base(instance, methodFinder)
        {
        }
    }
}