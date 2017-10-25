using System.Linq;
using System.Linq.Expressions;

namespace Firestorm.Engine
{
    /// <summary>
    /// Visits an expression, replacing the parameter expression with another.
    /// </summary>
    /// <remarks>Taken from http://stackoverflow.com/a/11160067/369247 </remarks>
    public class ParameterReplacerVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _source;
        private readonly ParameterExpression _target;

        public ParameterReplacerVisitor(ParameterExpression source, ParameterExpression target)
        {
            _source = source;
            _target = target;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters.Where(p => p != _source);
            return Expression.Lambda(Visit(node.Body), parameters);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _source ? _target : base.VisitParameter(node);
        }
    }
}