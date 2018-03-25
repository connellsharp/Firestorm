using System;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm
{
    public abstract class NewInstanceFixedConstructorReflectorBase<TInstance> : NewInstanceReflector<TInstance>
    {
        private readonly ConstructorInstanceGetter _instanceGetter;
        private readonly IEnumerable<Type> _parameterTypes;

        internal NewInstanceFixedConstructorReflectorBase(ConstructorInstanceGetter instanceGetter, params Type[] parameterTypes)
            : base(instanceGetter)
        {
            _instanceGetter = instanceGetter;
            _parameterTypes = parameterTypes;
        }

        public InstanceReflector<TInstance> WithArgumentsFromServiceProvider(IServiceProvider serviceProvider)
        {
            _instanceGetter.CtorArgs = _parameterTypes.Select(serviceProvider.GetService).ToArray();
            return new InstanceReflector<TInstance>(InstanceGetter);
        }
    }

    public class NewInstanceFixedConstructorReflector<TInstance, TArg1> : NewInstanceFixedConstructorReflectorBase<TInstance>
    {
        internal NewInstanceFixedConstructorReflector(ConstructorInstanceGetter instanceGetter)
            : base(instanceGetter, typeof(TArg1))
        {
        }
    }
    
    public class NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2> : NewInstanceFixedConstructorReflectorBase<TInstance>
    {
        internal NewInstanceFixedConstructorReflector(ConstructorInstanceGetter instanceGetter)
            : base(instanceGetter, typeof(TArg1), typeof(TArg2))
        {
        }
    }
    
    public class NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3> : NewInstanceFixedConstructorReflectorBase<TInstance>
    {
        internal NewInstanceFixedConstructorReflector(ConstructorInstanceGetter instanceGetter)
            : base(instanceGetter, typeof(TArg1), typeof(TArg2), typeof(TArg3))
        {
        }
    }
    
    public class NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4> : NewInstanceFixedConstructorReflectorBase<TInstance>
    {
        internal NewInstanceFixedConstructorReflector(ConstructorInstanceGetter instanceGetter)
            : base(instanceGetter, typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4))
        {
        }
    }
    
    public class NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5> : NewInstanceFixedConstructorReflectorBase<TInstance>
    {
        internal NewInstanceFixedConstructorReflector(ConstructorInstanceGetter instanceGetter)
            : base(instanceGetter, typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4), typeof(TArg5))
        {
        }
    }
    
    public class NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : NewInstanceFixedConstructorReflectorBase<TInstance>
    {
        internal NewInstanceFixedConstructorReflector(ConstructorInstanceGetter instanceGetter)
            : base(instanceGetter, typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4), typeof(TArg5), typeof(TArg6))
        {
        }
    }
}