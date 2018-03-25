using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm
{
    public abstract class StrongMethodReflectorBase<TInstance, TReturn> : MethodReflectorBase<TInstance, TReturn>
    {
        internal StrongMethodReflectorBase(TInstance instance, [NotNull] IMethodFinder methodFinder)
            : base(instance, methodFinder)
        {
        }

        public ServiceProviderMethodReflector<TInstance, TReturn> FromServiceProvider(IServiceProvider serviceProvider)
        {
            return new ServiceProviderMethodReflector<TInstance, TReturn>(Instance, MethodFinder, serviceProvider);
        }
    }

    public class StrongMethodReflector<TInstance, TReturn, TArg1> : StrongMethodReflectorBase<TInstance, TReturn>
    {
        internal StrongMethodReflector(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public TReturn Invoke(TArg1 arg1)
        {
            return base.Invoke(arg1);
        }
    }
    
    public class StrongMethodReflector<TInstance, TReturn, TArg1, TArg2> : StrongMethodReflectorBase<TInstance, TReturn>
    {
        internal StrongMethodReflector(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public TReturn Invoke(TArg1 arg1, TArg2 arg2)
        {
            return base.Invoke(arg1, arg2);
        }
    }
    
    public class StrongMethodReflector<TInstance, TReturn, TArg1, TArg2, TArg3> : StrongMethodReflectorBase<TInstance, TReturn>
    {
        internal StrongMethodReflector(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public TReturn Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return base.Invoke(arg1);
        }
    }
    
    public class StrongMethodReflector<TInstance, TReturn, TArg1, TArg2, TArg3, TArg4> : StrongMethodReflectorBase<TInstance, TReturn>
    {
        internal StrongMethodReflector(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public TReturn Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            return base.Invoke(arg1);
        }
    }
}