using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm.Engine
{
    /// <summary>
    /// Helper utility class for matching and setting string values from an expression property returning any type.
    /// </summary>
    public static class IdentifierExpressionHelpers
    {
        /// <summary>
        /// Returns a predicate that returns true if the item in the <see cref="identifierExpr"/> matches the <see cref="identifierObj"/>.
        /// </summary>
        [Pure]
        public static Expression<Func<TItem, bool>> GetIdentifierPredicate<TItem, TIdentifier>(Expression<Func<TItem, TIdentifier>> identifierExpr, TIdentifier identifierObj)
        {
            BinaryExpression predicateExpr = Expression.Equal(identifierExpr.Body, Expression.Constant(identifierObj, typeof(TIdentifier)));
            return Expression.Lambda<Func<TItem, bool>>(predicateExpr, identifierExpr.Parameters);
        }

        /// <summary>
        /// Returns a predicate that returns true if any of the items in the <see cref="identifierExpr"/> match the <see cref="identifier"/>.
        /// </summary>
        /// <remarks>
        /// Based on http://stackoverflow.com/a/326496/369247
        /// </remarks>
        [Pure]
        public static Expression<Func<TItem, bool>> GetAnyIdentifierPredicate<TItem, TIdentifier>(Expression<Func<TItem, IEnumerable<TIdentifier>>> identifierExpr, TIdentifier identifierObj)
        {
            var paramExpr = Expression.Parameter(typeof(TIdentifier), "r");
            BinaryExpression predicateExpr = Expression.Equal(paramExpr, Expression.Constant(identifierObj, typeof(TIdentifier)));
            var singleItemPredicate = Expression.Lambda<Func<TIdentifier, bool>>(predicateExpr, paramExpr);

            Type enumerableType = typeof(IEnumerable<TIdentifier>); // TODO handle List and arrays etc

            MethodInfo anyMethod = GetGenericMethod(typeof(Enumerable), "Any", new[] { typeof(TIdentifier) }, new[] { enumerableType, identifierExpr.Type }, BindingFlags.Static | BindingFlags.Public);
            var call = Expression.Call(anyMethod, identifierExpr.Body, singleItemPredicate);

            return Expression.Lambda<Func<TItem, bool>>(call, identifierExpr.Parameters);
        }

        private static MethodInfo GetGenericMethod(Type type, string name, Type[] typeArgs, Type[] paramTypes, BindingFlags flags)
        {
            IEnumerable<MethodInfo> methods = type.GetMethods(flags)
                .Where(m => m.Name == name)
                .Where(m => m.GetGenericArguments().Length == typeArgs.Length)
                .Where(m => m.GetParameters().Length == paramTypes.Length)
                .Select(m => m.MakeGenericMethod(typeArgs));

            return methods.First();
        }

        public static void SetIdentifier<TItem, TIdentifier>(TItem item, Expression<Func<TItem, TIdentifier>> identifierExpr, TIdentifier newIdentifier)
        {
            PropertyInfo propertyInfo = LambdaMemberUtilities.GetPropertyInfoFromLambda(identifierExpr);
            propertyInfo.SetValue(item, newIdentifier);
        }
    }
}