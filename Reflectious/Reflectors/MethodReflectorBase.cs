using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm
{
    public abstract class MethodReflectorBase<TInstance, TReturn>
    {
        protected readonly TInstance Instance;
        internal readonly IMethodFinder MethodFinder;

        internal MethodReflectorBase(TInstance instance, [NotNull] IMethodFinder methodFinder)
        {
            Instance = instance;
            MethodFinder = methodFinder ?? throw new ArgumentNullException(nameof(methodFinder));
        }

        [PublicAPI]
        public MethodInfo MethodInfo => MethodFinder.Find().GetMethodInfo();

        protected TReturn Invoke(params object[] args)
        {
            if(MethodFinder.WantsParameterTypes)
                MethodFinder.ParameterTypes = args.Select(a => a.GetType()).ToArray();
            
            IMethod method = MethodFinder.Find();
            
            object value = method.Invoke(Instance, args);
            return (TReturn) value;
        }
    }
}