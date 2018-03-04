using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Subs.Handlers
{
    public class NotSupportedFieldCollator<TItem> : IFieldCollator<TItem>
    {
        private readonly string _resourceType;

        public NotSupportedFieldCollator(string resourceType)
        {
            _resourceType = resourceType;
        }
        
        public Expression GetFilterExpression(ParameterExpression itemPram, FilterComparisonOperator comparisonOperator, string valueString)
        {
            throw new NotSupportedException("Filtering is not supported for " + _resourceType + ".");
        }

        public LambdaExpression GetSortExpression(ParameterExpression itemPram)
        {
            throw new NotSupportedException("Sorting is not supported for " + _resourceType + ".");
        }
    }
}