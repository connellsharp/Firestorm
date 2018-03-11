using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm
{
    public class StrongMethodInvoker<TInstance, TReturn, TArg1> : MethodInvoker<TInstance, TReturn>
    {
        internal StrongMethodInvoker(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public TReturn Invoke(TArg1 arg1)
        {
            return base.Invoke(arg1);
        }
    }
    
    public class StrongMethodInvoker<TInstance, TReturn, TArg1, TArg2> : MethodInvoker<TInstance, TReturn>
    {
        internal StrongMethodInvoker(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public TReturn Invoke(TArg1 arg1, TArg2 arg2)
        {
            return base.Invoke(arg1, arg2);
        }
    }
    
    public class StrongMethodInvoker<TInstance, TReturn, TArg1, TArg2, TArg3> : MethodInvoker<TInstance, TReturn>
    {
        internal StrongMethodInvoker(TInstance instance, [NotNull] IMethodFinder methodFinder) 
            : base(instance, methodFinder)
        {
        }

        [PublicAPI]
        public TReturn Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return base.Invoke(arg1);
        }
    }
    
    public class StrongMethodInvoker<TInstance, TReturn, TArg1, TArg2, TArg3, TArg4> : MethodInvoker<TInstance, TReturn>
    {
        internal StrongMethodInvoker(TInstance instance, [NotNull] IMethodFinder methodFinder) 
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