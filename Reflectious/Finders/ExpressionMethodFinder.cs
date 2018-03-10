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

        public Type[] GenericArguments
        {
            set => throw new NotSupportedException();
        }

        public Type[] ParameterTypes
        {
            set => throw new NotSupportedException();
        }

        public MethodInfo FindMethodInfo()
        {
            return _methodInfo;
        }

        public object FindAndInvoke(object instance, object[] args)
        {
            return _methodInfo.Invoke(instance, args);
        }
    }
}