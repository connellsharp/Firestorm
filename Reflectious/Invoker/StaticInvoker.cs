using System;
using JetBrains.Annotations;

namespace Firestorm
{
    public class StaticInvoker : InstanceInvoker<object>
    {
        internal StaticInvoker(Type type)
            : base(type)
        {
        }

        [PublicAPI]
        public InstanceInvoker WithInstance(object instance)
        {
            return new InstanceInvoker(instance);
        }
    }

    public class StaticInvoker<TType> : InstanceInvoker<TType>
        where TType : class
    {
        public StaticInvoker()
            : base(typeof(TType))
        {
        }

        [PublicAPI]
        public InstanceInvoker<TType> WithInstance(TType instance)
        {
            return new InstanceInvoker<TType>(instance);
        }
    }
}