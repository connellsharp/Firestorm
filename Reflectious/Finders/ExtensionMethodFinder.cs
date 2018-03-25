using System;
using System.Linq;

namespace Firestorm
{
    internal class ExtensionMethodFinder : IMethodFinder
    {
        private readonly IMethodFinder _underlyingFinder;
        private readonly Type[] _extensionThisParamTypeArr;

        public ExtensionMethodFinder(IMethodFinder underlyingFinder, Type extensionThisParamType)
        {
            _underlyingFinder = underlyingFinder;
            _extensionThisParamTypeArr = new[] { extensionThisParamType };
        }

        public Type[] GenericArguments
        {
            get { return _underlyingFinder.GenericArguments; }
            set { _underlyingFinder.GenericArguments = value; }
        }

        public Type[] ParameterTypes
        {
            get { return _underlyingFinder.ParameterTypes.Skip(1).ToArray(); }
            set { _underlyingFinder.ParameterTypes = _extensionThisParamTypeArr.Concat(value).ToArray(); }
        }

        public bool WantsParameterTypes
        {
            get { return _underlyingFinder.WantsParameterTypes; }
        }

        public IMethod Find()
        {           
            return _underlyingFinder.Find();
        }
    }
}