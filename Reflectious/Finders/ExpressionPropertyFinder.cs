using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm
{
    public class ExpressionPropertyFinder<TSource, TReturn> : IPropertyFinder
    {
        private readonly PropertyInfo _propertyInfo;

        public ExpressionPropertyFinder(Expression<Func<TSource, TReturn>> propertyExpression)
        {
            _propertyInfo = LambdaMemberUtilities.GetPropertyInfoFromLambda(propertyExpression);
        }

        public PropertyInfo Find()
        {
            return _propertyInfo;
        }

        public Type PropertyType
        {
            get { return Find().PropertyType; }
        }
    }
}