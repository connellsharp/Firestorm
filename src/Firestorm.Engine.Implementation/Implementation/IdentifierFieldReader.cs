using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine
{
    internal class IdentifierFieldReader<TItem> : IFieldReader<TItem>
    {
        private readonly IIdentifierInfo<TItem> _identifierInfo;

        public IdentifierFieldReader(IIdentifierInfo<TItem> identifierInfo)
        {
            _identifierInfo = identifierInfo;
        }

        public Type FieldType => _identifierInfo.IdentifierType;

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            return _identifierInfo.GetGetterExpression(itemPram);
        }

        public IFieldValueReplacer<TItem> Replacer { get; } = null;
    }
}