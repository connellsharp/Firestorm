using System;
using Firestorm.Data;
using Firestorm.Engine.Subs.Context;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Resolving.Factories;

namespace Firestorm.Stems.Fuel.Essential.Factories
{
    internal class DelegateItemLocatorFactory<TItem> : IFactory<IItemLocator<TItem>, TItem>
        where TItem : class
    {
        private readonly FieldDefinitionHandlerPart.GetInstanceMethodDelegate _getInstanceLocatorMethod;

        public DelegateItemLocatorFactory(FieldDefinitionHandlerPart.GetInstanceMethodDelegate getInstanceLocatorMethod)
        {
            _getInstanceLocatorMethod = getInstanceLocatorMethod;
        }

        public IItemLocator<TItem> Get(Stem<TItem> stem)
        {
            Delegate instanceMethod = _getInstanceLocatorMethod.Invoke(stem);
            return new SubstemItemLocator(instanceMethod);
        }

        private class SubstemItemLocator : IItemLocator<TItem>
        {
            private readonly Delegate _instanceMethod;

            public SubstemItemLocator(Delegate instanceMethod)
            {
                _instanceMethod = instanceMethod;
            }

            public TItem TryLocateItem(IEngineRepository<TItem> navRepository, object findValue)
            {
                throw new NotImplementedException("Not got this far yet.");

                // Not sure what to do here. Stem method can't accept a repository.

                object loadedParentItem = null;
                var ret = _instanceMethod.DynamicInvoke(findValue, loadedParentItem);
                return (TItem)ret;
            }
        }
    }
}