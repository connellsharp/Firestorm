using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Additives.Readers
{
    /// <summary>
    /// Field reader for instance methods coupled with an expression.
    /// The expression is executed as part of the query and the method is executed afterwards in the application.
    /// </summary>
    public class ExpressionMiddleDelegateFieldReader<TItem, TMiddle, TValue> : IFieldReader<TItem>
    {
        private readonly Expression<Func<TItem, TMiddle>> _middleExpression;

        public ExpressionMiddleDelegateFieldReader(Expression<Func<TItem, TMiddle>> middleExpression, Func<TMiddle, TValue> delegateMethod)
        {
            _middleExpression = middleExpression;

            Replacer = new ExpressionMiddleReplacer(delegateMethod);
        }

        public Type FieldType { get; } = typeof(TValue);

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            var replacer = new ParameterReplacerVisitor(_middleExpression.Parameters[0], itemPram);
            return replacer.Visit(_middleExpression.Body);
        }

        public IFieldValueReplacer<TItem> Replacer { get; }

        private class ExpressionMiddleReplacer : IFieldValueReplacer<TItem>
        {
            private readonly Func<TMiddle, TValue> _delegateMethod;

            public ExpressionMiddleReplacer(Func<TMiddle, TValue> delegateMethod)
            {
                _delegateMethod = delegateMethod;
            }

            public Task LoadAsync(IQueryable<TItem> itemsQuery)
            {
                return Task.FromResult(false);
            }

            public object GetReplacement(object dbValue)
            {
                return _delegateMethod((TMiddle)dbValue);
            }
        }
    }
}