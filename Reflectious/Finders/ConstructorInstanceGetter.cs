using System;

namespace Firestorm
{
    public class ConstructorInstanceGetter : IInstanceGetter
    {
        private readonly Func<object[], object> _createInstance;
        internal object[] CtorArgs = new object[0];

        public ConstructorInstanceGetter(Func<object[], object> createInstance, Type type)
        {
            _createInstance = createInstance;
            Type = type;
        }

        public Type Type { get; }
        
        public object GetInstance()
        {
            return _createInstance(CtorArgs);
        }
    }
}