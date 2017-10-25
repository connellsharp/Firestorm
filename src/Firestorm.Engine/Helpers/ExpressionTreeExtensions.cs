using System;
using System.Linq.Expressions;

namespace Firestorm.Engine
{
    public static class ExpressionTreeExtensions
    {
        // from http://stackoverflow.com/a/33442945/369247
        public static Expression<Func<TFrom, TTo>> Chain<TFrom, TMiddle, TTo>(this Expression<Func<TFrom, TMiddle>> first, Expression<Func<TMiddle, TTo>> second)
        {
            Expression visitedSecond = new SwapVisitor(second.Parameters[0], first.Body).Visit(second.Body);
            return Expression.Lambda<Func<TFrom, TTo>>(visitedSecond, first.Parameters);
        }

        // weakly-typed version of the above method
        public static LambdaExpression Chain(this LambdaExpression first, LambdaExpression second)
        {
            Expression visitedSecond = new SwapVisitor(second.Parameters[0], first.Body).Visit(second.Body);
            return Expression.Lambda(visitedSecond, first.Parameters);
        }

        public static Expression<Func<TFrom, TCasted>> CastBody<TFrom, TOriginal, TCasted>(this Expression<Func<TFrom, TOriginal>> lambda)
        {
            UnaryExpression convertedBody = Expression.Convert(lambda.Body, typeof(TCasted));
            Expression visitedBody = new SwapVisitor(lambda.Body, convertedBody).Visit(lambda.Body);
            return Expression.Lambda<Func<TFrom, TCasted>>(visitedBody, lambda.Parameters);
        }

        public static LambdaExpression CastBody(this LambdaExpression lambda, Type toReturnType)
        {
            UnaryExpression convertedBody = Expression.Convert(lambda.Body, toReturnType);
            Expression visitedBody = new SwapVisitor(lambda.Body, convertedBody).Visit(lambda.Body);
            return Expression.Lambda(visitedBody, lambda.Parameters);
        }

        private class SwapVisitor : ExpressionVisitor
        {
            private readonly Expression _from;
            private readonly Expression _to;

            public SwapVisitor(Expression from, Expression to)
            {
                _from = from;
                _to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == _from ? _to : base.Visit(node);
            }
        }
    }
}