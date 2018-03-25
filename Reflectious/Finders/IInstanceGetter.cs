using System;
using JetBrains.Annotations;

namespace Firestorm
{
    internal interface IInstanceGetter
    {
        Type Type { get; }
        
        object GetInstance();
    }

    internal class WeakInstanceGetter : IInstanceGetter
    {
        private readonly object _instance;

        public WeakInstanceGetter(object instance)
        {
            _instance = instance;
            Type = _instance.GetType();
        }
        
        public Type Type { get; }
        
        public object GetInstance()
        {
            return _instance;
        }
    }

    internal class StrongInstanceGetter<TInstance> : IInstanceGetter
    {
        [NotNull] private readonly TInstance _instance;

        public StrongInstanceGetter([NotNull] TInstance instance)
        {
            _instance = instance;
        }

        public Type Type
        {
            get { return typeof(TInstance); }
        }

        public object GetInstance()
        {
            return _instance;
        }
    }

    internal class StrongInstanceFuncGetter<TInstance> : IInstanceGetter
    {
        private readonly Func<TInstance> _getInstanceFunc;

        public StrongInstanceFuncGetter([NotNull] Func<TInstance> getInstanceFunc)
        {
            _getInstanceFunc = getInstanceFunc;
        }

        public Type Type
        {
            get { return typeof(TInstance); }
        }

        public object GetInstance()
        {
            return _getInstanceFunc();
        }
    }

    /// <summary>
    /// Instance getter returning no instance. Used for static members.
    /// </summary>
    internal class NullInstanceGetter : IInstanceGetter
    {
        internal NullInstanceGetter(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
        
        public object GetInstance()
        {
            return null;
        }
    }
}