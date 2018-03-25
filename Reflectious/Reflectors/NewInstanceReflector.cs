using System;

namespace Firestorm
{
    public class NewInstanceReflector<TInstance> : InstanceReflector<TInstance>
    {
        private readonly ConstructorInstanceGetter _instanceGetter;

        internal NewInstanceReflector(ConstructorInstanceGetter instanceGetter)
            : base(instanceGetter)
        {
            _instanceGetter = instanceGetter;
        }

        public NewInstanceReflector<TInstance> UsingConstructorArguments(params object[] ctorArgs)
        {
            _instanceGetter.CtorArgs = ctorArgs;
            return this;
        }

        public NewInstanceFixedConstructorReflector<TInstance, TArg1> UsingConstructor<TArg1>()
        {
            return new NewInstanceFixedConstructorReflector<TInstance, TArg1>(_instanceGetter);
        }

        public NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2> UsingConstructor<TArg1, TArg2>()
        {
            return new NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2>(_instanceGetter);
        }

        public NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3> UsingConstructor<TArg1, TArg2, TArg3>()
        {
            return new NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3>(_instanceGetter);
        }

        public NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4> UsingConstructor<TArg1, TArg2, TArg3, TArg4>()
        {
            return new NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4>(_instanceGetter);
        }

        public NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5> UsingConstructor<TArg1, TArg2, TArg3, TArg4, TArg5>()
        {
            return new NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5>(_instanceGetter);
        }

        public NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> UsingConstructor<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>()
        {
            return new NewInstanceFixedConstructorReflector<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(_instanceGetter);
        }

        public InstanceReflector<TInstance> FromServiceProvider(IServiceProvider serviceProvider)
        {
            if (_instanceGetter.CtorArgs.Length > 0)
            {
                var getter = new ServiceProviderInstanceGetter(serviceProvider, InstanceGetter.Type);
                return new InstanceReflector<TInstance>(getter);
            }
            else
            {
                var getter = new ServiceProviderInstanceGetter(serviceProvider, InstanceGetter.Type);
                return new InstanceReflector<TInstance>(getter);
            }
        }
    }
}