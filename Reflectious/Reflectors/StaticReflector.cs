using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm
{
    public class StaticReflector : StaticReflector<object>
    {
        internal StaticReflector(Type type)
            : base(type)
        {
        }

        [PublicAPI]
        public new InstanceReflector WithInstance(object instance)
        {
            return new InstanceReflector(instance);
        }

        [PublicAPI]
        public new MethodReflector GetConstructor()
        {
            return new MethodReflector(null, new ConstructorFinder(Type));
        }
    }

    public class StaticReflector<TType> : InstanceReflector<TType>
        where TType : class
    {
        internal StaticReflector(Type type)
            : base(type)
        {
            Debug.Assert(typeof(TType).IsAssignableFrom(type));
        }
        
        public StaticReflector()
            : base(typeof(TType))
        {
        }

        [PublicAPI]
        public MethodReflector<TType, TType> GetConstructor()
        {
            return new MethodReflector<TType, TType>(null, new ConstructorFinder(Type));
        }

        [PublicAPI]
        public TType CreateInstance(params object[] args)
        {
            return GetConstructor()
                //.WithParameters(args.Select(a => a?.GetType())) // already happening in .Invoke
                .Invoke(args);
        }

        [PublicAPI]
        public InstanceReflector<TType> WithNewInstance()
        {
            return WithInstance(CreateInstance());
        }

        [PublicAPI]
        public InstanceReflector<TType> WithInstance(TType instance)
        {
            return new InstanceReflector<TType>(instance);
        }

        [PublicAPI]
        public StaticReflector MakeGeneric(params Type[] types)
        {
            var genericType = Type.MakeGenericType(types);
            return new StaticReflector(genericType);
        }
        
        [PublicAPI]
        public StaticReflector MakeGeneric<T1>()
        {
            return MakeGeneric(typeof(T1));
        }

        [PublicAPI]
        public StaticReflector MakeGeneric<T1, T2>()
        {
            return MakeGeneric(typeof(T1), typeof(T2));
        }

        [PublicAPI]
        public StaticReflector MakeGeneric<T1, T2, T3>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3));
        }

        [PublicAPI]
        public StaticReflector MakeGeneric<T1, T2, T3, T4>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        [PublicAPI]
        public StaticReflector MakeGeneric<T1, T2, T3, T4, T5>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        [PublicAPI]
        public StaticReflector MakeGeneric<T1, T2, T3, T4, T5, T6>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        }

        [PublicAPI]
        public StaticReflector MakeGeneric(IEnumerable<Type> types)
        {
            return MakeGeneric(types.ToArray());
        }
    }
}