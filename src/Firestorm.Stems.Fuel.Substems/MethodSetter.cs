using System;
using Firestorm.Engine.Subs;
using Firestorm.Stems.Attributes.Definitions;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Substems
{
    /// <summary>
    /// An <see cref="INavigationSetter{TParent,TNav}"/> implementation that uses Stem methods (instance and static) 
    /// to set the navgation property to an item that was likely built using a substem.
    /// </summary>
    internal class MethodSetter<TItem, TNav> : INavigationSetter<TItem, TNav>
        where TItem : class
    {
        private readonly Action<TItem, TNav> _setterAction;

        public MethodSetter(Action<TItem, TNav> setterAction)
        {
            _setterAction = setterAction;
        }

        public MethodSetter(FieldDefinitionHandlerPart definitionSetter, Stem<TItem> stem)
        {
            Delegate deleg = definitionSetter.GetInstanceMethod.Invoke(stem);
            _setterAction = (Action<TItem, TNav>)deleg;
        }

        [CanBeNull]
        public static MethodSetter<TItem, TNav> FromDefinition(FieldDefinition definition, Stem<TItem> stem)
        {
            if (definition.Setter.GetInstanceMethod == null)
                return null;

            return new MethodSetter<TItem, TNav>(definition.Setter, stem);
        }

        public void SetNavItem(TItem parent, TNav item)
        {
            _setterAction(parent, item);
        }
    }
}