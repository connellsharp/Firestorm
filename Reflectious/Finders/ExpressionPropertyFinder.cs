using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm
{
    internal class ExpressionPropertyFinder<TSource, TReturn> : IPropertyFinder
    {
        private readonly PropertyInfo _propertyInfo;

        public ExpressionPropertyFinder(Expression<Func<TSource, TReturn>> propertyExpression)
        {
            _propertyInfo = LambdaMemberUtilities.GetPropertyInfoFromLambda(propertyExpression);
        }

        public IProperty Find()
        {
            return new ReflectionProperty(_propertyInfo);
        }

        public Type PropertyType
        {
            get { return _propertyInfo.PropertyType; }
        }
    }
}