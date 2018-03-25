using System;
using System.Linq;

namespace Firestorm
{
    internal class CachedMethodFinder : IMethodFinder
    {
        private readonly ICacheableMethodFinder _underlyingFinder;

        public CachedMethodFinder(ICacheableMethodFinder underlyingFinder)
        {
            _underlyingFinder = underlyingFinder;
        }

        public Type[] GenericArguments
        {
            get { return _underlyingFinder.GenericArguments; }
            set
            {
                if (FoundItem != null && !_underlyingFinder.GenericArguments.SequenceEqual(value))
                    throw new Exception("Cannot alter GenericArguments of cached item.");

                _underlyingFinder.GenericArguments = value;
            }
        }

        public Type[] ParameterTypes
        {
            get { return _underlyingFinder.ParameterTypes; }
            set
            {
                if(FoundItem != null && !_underlyingFinder.ParameterTypes.SequenceEqual(value))
                    throw new Exception("Cannot alter ParameterTypes of cached item.");

                _underlyingFinder.ParameterTypes = value;
            }
        }

        public bool WantsParameterTypes => _underlyingFinder.WantsParameterTypes;

        public IMethod Find()
        {
            if (FoundItem != null)
                return FoundItem;

            return FoundItem = _underlyingFinder.Find();
        }

        public IMethod FoundItem { get; private set; }
    }
}