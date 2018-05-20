using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Reflectious;

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
            return identifierExpr.ChainEquals(identifierObj);
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
            MethodInfo anyMethod = typeof(Enumerable).Reflect()
                .GetMethod("Any")
                .ReturnsType<bool>()
                .MakeGeneric<TIdentifier>()
                .WithParameters<IEnumerable<TIdentifier>, Func<TIdentifier, bool>>() // TODO handle List and arrays etc ?
                .MethodInfo;

            var singleItemPredicate = Expressions.Identity<TIdentifier>().ChainEquals(identifierObj);
            
            var call = Expression.Call(anyMethod, identifierExpr.Body, singleItemPredicate);
            return Expression.Lambda<Func<TItem, bool>>(call, identifierExpr.Parameters);
        }

        public static void SetIdentifier<TItem, TIdentifier>(TItem item, Expression<Func<TItem, TIdentifier>> identifierExpr, TIdentifier newIdentifier)
        {
            item.Reflect().GetProperty(identifierExpr).SetValue(newIdentifier);
        }
    }
}