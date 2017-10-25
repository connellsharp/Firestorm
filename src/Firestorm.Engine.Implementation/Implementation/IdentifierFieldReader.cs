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

        public Expression GetFilterExpression(ParameterExpression itemPram, FilterComparisonOperator comparisonOperator, string valueString)
        {
            throw new NotSupportedException("Cannot filter with identifier info readers.");
        }

        public LambdaExpression GetSortExpression(ParameterExpression itemPram)
        {
            var getterExpression = _identifierInfo.GetGetterExpression(itemPram);
            return Expression.Lambda(getterExpression, itemPram);
        }
    }
}