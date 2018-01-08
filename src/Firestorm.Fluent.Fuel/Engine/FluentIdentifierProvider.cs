using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Identifiers;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Engine
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Similar to <see cref="Stems.AttributeIdentifierProvider{TItem}"/></remarks>
    internal class FluentIdentifierProvider<TItem> : IIdentifierProvider<TItem>
        where TItem : class
    {
        private readonly IReadOnlyDictionary<string, ApiIdentifierModel<TItem>> _identifierModels;

        public FluentIdentifierProvider(IEnumerable<ApiIdentifierModel<TItem>> identifierModels)
        {
            _identifierModels = new LazyDictionary<ApiIdentifierModel<TItem>>(identifierModels, i => i.Name);
        }

        public IIdentifierInfo<TItem> GetInfo(string identifierName)
        {
            if (identifierName == null)
            {
                if (_identifierModels.Count == 0)
                    return new IDConventionIdentifierInfo<TItem>();

                var infos = _identifierModels.Values.Select(m => m.IdentifierInfo);
                return new CombinedIdentifierInfo<TItem>(infos);
            }
            else
            {
                var definition = _identifierModels[identifierName];

                return definition.IdentifierInfo;
            }
        }
    }
}