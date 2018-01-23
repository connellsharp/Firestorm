using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives
{
    internal static class PropertyInfoUtilities
    {
        internal static void EnsureValidProperty<TItem>([NotNull] PropertyInfo propertyInfo) 
            where TItem : class
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            Type expectedType = typeof(TItem);
            if (!propertyInfo.ReflectedType.IsAssignableFrom(expectedType))
                throw new ArgumentException(String.Format("Property '{0}' was reflected from a type that is not assignable from {1}.", propertyInfo.Name, expectedType));
        }

        /// <remarks>
        /// Taken from http://stackoverflow.com/a/672212/369247
        /// Very similar utility in <see cref="IdentifierExpressionHelpers"/>
        /// </remarks>
        internal static PropertyInfo GetPropertyInfoFromLambda<TItem, TProperty>([NotNull] Expression<Func<TItem, TProperty>> propertyExpression) 
            where TItem : class
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException(String.Format("Expression '{0}' refers to a method, not a property.", propertyExpression));

            var propInfo = memberExpr.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(String.Format("Expression '{0}' refers to a field, not a property.", propertyExpression));

            return propInfo;
        }
    }
}