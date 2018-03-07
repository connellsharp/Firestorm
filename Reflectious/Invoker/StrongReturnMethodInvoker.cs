using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm
{
    public class StrongReturnMethodInvoker<TInstance, TReturn> : MethodInvoker
    {
        [NotNull] private readonly Expression<Func<TInstance, TReturn>> _expression;

        public StrongReturnMethodInvoker(TInstance instance, [NotNull] Expression<Func<TInstance, TReturn>> expression)
            : base(instance, LambdaMemberUtilities.GetMethodInfoFromLambda(expression))
        {
            _expression = expression;
        }
    }
}