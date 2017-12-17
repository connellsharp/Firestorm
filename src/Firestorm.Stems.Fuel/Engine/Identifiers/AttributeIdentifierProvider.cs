using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Identifiers;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Definitions;

namespace Firestorm.Stems.Fuel.Identifiers
{
    /// <summary>
    /// Builds <see cref="IIdentifierInfo{TItem}"/> from <see cref="IdentifierAttribute"/>s placed on properties and predicate methods in a Stem.
    /// </summary>
    internal class AttributeIdentifierProvider<TItem> : IIdentifierProvider<TItem>
        where TItem : class
    {
        private readonly Stem<TItem> _stem;
        private readonly StemDefinition _stemDefinition;

        public AttributeIdentifierProvider(Stem<TItem> stem)
        {
            _stem = stem;

            var analyzerFactory = stem.Configuration.AnalyzerCache;
            var analyzer = analyzerFactory.GetAnalyzer<AttributeAnalyzer>(stem.GetType(), stem.Configuration);
            _stemDefinition = analyzer.Definition;
        }

        public IIdentifierInfo<TItem> GetInfo(string identifierName)
        {
            if (identifierName == null)
            {
                if(_stemDefinition.IdentifierDefinitions.Count == 0)
                    return new IDConventionIdentifierInfo<TItem>();

                var infos = _stemDefinition.IdentifierDefinitions.Values.Select(GetIdentifierInfoFromDefinition);
                return new CombinedIdentifierInfo<TItem>(infos);
            }
            else
            {
                IdentifierDefinition definition = _stemDefinition.IdentifierDefinitions[identifierName];

                return GetIdentifierInfoFromDefinition(definition);
            }
        }

        private IIdentifierInfo<TItem> GetIdentifierInfoFromDefinition(IdentifierDefinition definition)
        {
            if (definition.GetterMethod != null)
                return new MethodIdentifierInfo<TItem>(definition.GetterMethod, _stem);

            if (definition.ExactGetterMethod != null && definition.ExactValue != null)
                return new ExactIdentifierInfo<TItem>(definition.ExactValue, _stem, definition.ExactGetterMethod);

            if (definition.GetterExpression != null)
                return new ExpressionAndSetterIdentifierInfo<TItem>(definition.GetterExpression, (Action<TItem, string>) definition.SetterAction, definition.IsMultiReference);

            if (definition.PredicateMethod != null)
                return new PredicateMethodIdentifierInfo<TItem>(definition.PredicateMethod, (Action<TItem, string>) definition.SetterAction);

            throw new StemAttributeSetupException("Identifier attribute definition was setup incorrectly.");
        }
    }
}