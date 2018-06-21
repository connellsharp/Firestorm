using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Additives.Readers
{
    /// <summary>
    /// Base class for basic fields that don't need database return values replacing with more complex objects.
    /// </summary>
    public abstract class BasicFieldReaderBase<TItem> : IFieldReader<TItem>
        where TItem : class
    {
        public abstract Type FieldType { get; }

        protected abstract Expression GetGetterExpression(ParameterExpression parameterExpr);

        public IFieldValueReplacer<TItem> Replacer { get; } = null;

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            return GetGetterExpression(itemPram);
        }
    }
}
