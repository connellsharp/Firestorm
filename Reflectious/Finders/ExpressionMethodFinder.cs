using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm
{
    public class ExpressionMethodFinder<TSource, TReturn> : IMethodFinder
    {
        private readonly MethodInfo _methodInfo;

        public ExpressionMethodFinder(Expression<Func<TSource, TReturn>> expression)
        {
            _methodInfo = LambdaMemberUtilities.GetMethodInfoFromLambda(expression);
        }

        public MethodInfo Find()
        {
            return _methodInfo;
        }

        public Type[] GenericArguments
        {
            set => throw new NotSupportedException();
        }

        public Type[] ParameterTypes
        {
            set => throw new NotSupportedException();
        }
    }
}