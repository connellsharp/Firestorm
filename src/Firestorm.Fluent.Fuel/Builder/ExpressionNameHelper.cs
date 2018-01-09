using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Firestorm.Fluent.Fuel.Builder
{
    /// <remarks>
    /// Ripped straight from https://stackoverflow.com/a/3269649/369247.
    /// There are some similarities between some methods in
    ///   <see cref="Firestorm.Engine.IdentifierExpressionHelpers"/> and also <see cref="Firestorm.Engine.Additives.PropertyInfoUtilities"/>.
    /// </remarks>
    internal static class ExpressionNameHelper
    {
        internal static string GetFullPropertyName<T, TProperty>(Expression<Func<T, TProperty>> exp)
        {
            if (!TryFindMemberExpression(exp.Body, out MemberExpression memberExp))
                return string.Empty;

            var memberNames = new Stack<string>();

            do
            {
                memberNames.Push(memberExp.Member.Name);
            }
            while (TryFindMemberExpression(memberExp.Expression, out memberExp));

            return string.Join(".", memberNames.ToArray());
        }
        
        private static bool TryFindMemberExpression(Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;
            if (memberExp != null)
                return true;

            // if the compiler created an automatic conversion, it'll look something like...
            // obj => Convert(obj.Property) [e.g., int -> object]             OR:
            // obj => ConvertChecked(obj.Property) [e.g., int -> long]
            // ...which are the cases checked in IsConversion
            if (IsConversion(exp) && exp is UnaryExpression)
            {
                memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
                if (memberExp != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConversion(Expression exp)
        {
            return (
                exp.NodeType == ExpressionType.Convert ||
                exp.NodeType == ExpressionType.ConvertChecked
            );
        }
    }
}