using System;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Identifiers;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Fuel.Identifiers;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Essentials.Factories.Factories
{
    internal class IdentifierFactory<TItem> : IFactory<IIdentifierInfo<TItem>, TItem>
        where TItem : class
    {
        private readonly IdentifierDefinition _definition;

        public IdentifierFactory(IdentifierDefinition definition)
        {
            _definition = definition;
        }

        public IIdentifierInfo<TItem> Get(Stem<TItem> stem)
        {
            if (_definition.GetterMethod != null)
                return new MethodIdentifierInfo<TItem>(_definition.GetterMethod, stem);

            if (_definition.ExactGetterMethod != null && _definition.ExactValue != null)
                return new ExactIdentifierInfo<TItem>(_definition.ExactValue, stem, _definition.ExactGetterMethod);

            if (_definition.GetterExpression != null)
                return new ExpressionAndSetterIdentifierInfo<TItem>(_definition.GetterExpression, (Action<TItem, string>) _definition.SetterAction, _definition.IsMultiReference);

            if (_definition.PredicateMethod != null)
                return new PredicateMethodIdentifierInfo<TItem>(_definition.PredicateMethod, (Action<TItem, string>) _definition.SetterAction);

            throw new StemAttributeSetupException("Identifier attribute definition was setup incorrectly.");
        }
    }
}