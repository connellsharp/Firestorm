using System;
using System.Collections.Generic;
using Firestorm.Engine.Identifiers;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Engine
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Would be similar to <see cref="Stems.AttributeIdentifierProvider{TItem}"/></remarks>
    internal class FluentIdentifierProvider<TItem> : IIdentifierProvider<TItem>
    {
        private readonly IDictionary<string, ApiIdentifierModel<TItem>> _identifierModels;

        public FluentIdentifierProvider(IDictionary<string, ApiIdentifierModel<TItem>> identifierModels)
        {
            _identifierModels = identifierModels;
        }

        public IIdentifierInfo<TItem> GetInfo(string identifierName)
        {
            //if(_identifierModels.ContainsKey(identifierName))
            throw new NotImplementedException();
        }
    }
}