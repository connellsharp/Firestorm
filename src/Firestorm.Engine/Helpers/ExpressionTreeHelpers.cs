using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm.Engine
{
    public static class ExpressionTreeHelpers
    {
        [Pure]
        public static IQueryable GetSelectExpressionQuery(IQueryable sourceQuery, ParameterExpression itemPram, Expression selectExpression)
        {
            LambdaExpression selectorLambda = Expression.Lambda(selectExpression, itemPram);
            return GetSelectExpressionQuery(sourceQuery, selectorLambda);
        }

        [Pure]
        public static IQueryable GetSelectExpressionQuery(IQueryable sourceQuery, LambdaExpression selectorLambda)
        {
            Debug.Assert(selectorLambda.Parameters.Count == 1);
            Debug.Assert(sourceQuery.ElementType == selectorLambda.Parameters[0].Type);

            MethodCallExpression selectMethodExpr = Expression.Call(typeof(Queryable), "Select", new[] { sourceQuery.ElementType, selectorLambda.ReturnType }, sourceQuery.Expression, selectorLambda);
            return sourceQuery.Provider.CreateQuery(selectMethodExpr);
        }

        [Pure]
        public static IQueryable<TItem> GetOrderByExpressionQuery<TItem>(IQueryable<TItem> sourceQuery, LambdaExpression selectorLambda, SortDirection direction = SortDirection.Unspecified, bool thenBy = false)
            where TItem : class
        {
            Debug.Assert(selectorLambda.Parameters.Count == 1);
            Debug.Assert(sourceQuery.ElementType == selectorLambda.Parameters[0].Type);

            string methodName = thenBy ? "ThenBy" : "OrderBy";
            if (direction == SortDirection.Descending)
                methodName += "Descending";

            MethodCallExpression orderMethodExpr = Expression.Call(typeof(Queryable), methodName, new[] { sourceQuery.ElementType, selectorLambda.ReturnType }, sourceQuery.Expression, selectorLambda);

            IQueryable query = sourceQuery.Provider.CreateQuery(orderMethodExpr);
            return (IQueryable<TItem>)query;
        }

        [Pure]
        public static Expression NullConditional(Expression expressionBody, ParameterExpression parameter)
        {
            BinaryExpression nullCheck = Expression.Equal(parameter, Expression.Constant(null, parameter.Type));
            var nullCondition = Expression.Condition(nullCheck, Expression.Constant(null, expressionBody.Type), expressionBody);
            return nullCondition;
        }
    }
}