using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This functionality was supposed to be for the <see cref="AttributeIdentifierProvider"/> in Stems.
    /// </remarks>
    public class IdentifierDictionaryInfoFactory<TItem, TDefinition>
        where TItem : class
    {
        readonly IDictionary<string, TDefinition> _definitions;
        private readonly Func<TDefinition, IIdentifierInfo<TItem>> _getIdentifierInfoFromDefinition;

        public IdentifierDictionaryInfoFactory(IDictionary<string, TDefinition> definitions, Func<TDefinition, IIdentifierInfo<TItem>> getIdentifierInfoFromDefinition)
        {
            _definitions = definitions;
            _getIdentifierInfoFromDefinition = getIdentifierInfoFromDefinition;
        }

        public IIdentifierInfo<TItem> GetInfo(string identifierName)
        {
            if (identifierName == null)
            {
                if (_definitions.Count == 0)
                    return new IDConventionIdentifierInfo<TItem>();

                var infos = _definitions.Values.Select(_getIdentifierInfoFromDefinition);
                return new CombinedIdentifierInfo<TItem>(infos);
            }
            else
            {
                TDefinition definition = _definitions[identifierName];

                return _getIdentifierInfoFromDefinition(definition);
            }
        }
    }
}