using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Readers
{
    /// <summary>
    /// Uses a getter <see cref="Expression"/> to read the field.
    /// </summary>
    public class ExpressionFieldReader<TItem, TValue> : BasicFieldReaderBase<TItem>
        where TItem : class
    {
        private readonly Expression<Func<TItem, TValue>> _getterExpression;

        public ExpressionFieldReader([NotNull] Expression<Func<TItem, TValue>> getterExpression)
        {
            if (getterExpression == null)
                throw new ArgumentNullException(nameof(getterExpression));

            _getterExpression = getterExpression;
        }

        protected override Expression GetGetterExpression(ParameterExpression parameterExpr)
        {
            var replacer = new ParameterReplacerVisitor(_getterExpression.Parameters[0], parameterExpr);
            return replacer.Visit(_getterExpression.Body);
        }

        public override Type FieldType
        {
            get { return typeof(TValue); }
        }
    }
}