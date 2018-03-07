using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm
{
    /// <remarks>
    /// Taken from http://stackoverflow.com/a/672212 and http://stackoverflow.com/a/17116267
    /// </remarks>
    public class LambdaMemberUtilities
    {
        [Pure]
        private static MemberExpression GetMemberExpression<TObj, TReturn>(Expression<Func<TObj, TReturn>> memberExpression)
        {
            if (memberExpression == null)
                throw new ArgumentNullException(nameof(memberExpression));
            
            // sometimes the expression comes in as Convert(originalexpression)
            var unExp = memberExpression.Body as UnaryExpression;
            if (unExp != null)
            {
                var operand = unExp.Operand as MemberExpression;
                if (operand != null)
                    return operand;

                throw new ArgumentException();
            }

            var expr = memberExpression.Body as MemberExpression;
            if (expr != null)
                return expr;

            throw new ArgumentException();
        }
        
        public static MethodInfo GetMethodInfoFromLambda<TObj, TReturn>([NotNull] Expression<Func<TObj, TReturn>> methodCallExpression) 
        {
            if (methodCallExpression == null)
                throw new ArgumentNullException(nameof(methodCallExpression));

            var methodCallExpr = methodCallExpression.Body as MethodCallExpression;
            if (methodCallExpr == null)
                throw new ArgumentException(String.Format("Expression '{0}' does not refer to a method.", methodCallExpression));

            return methodCallExpr.Method;
        }
        
        public static MemberInfo GetMemberInfoFromLambda<TObj, TReturn>([NotNull] Expression<Func<TObj, TReturn>> memberExpression) 
        {
            var memberExpr = GetMemberExpression(memberExpression);
            if (memberExpr == null)
                throw new ArgumentException(String.Format("Expression '{0}' refers to a method, not a property.", memberExpression));

            return memberExpr.Member;
        }
        
        public static PropertyInfo GetPropertyInfoFromLambda<TObj, TReturn>([NotNull] Expression<Func<TObj, TReturn>> propertyExpression) 
        {
            var propInfo = GetMemberInfoFromLambda(propertyExpression) as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(String.Format("Expression '{0}' refers to a field, not a property.", propertyExpression));

            return propInfo;
        }
        
        public static FieldInfo GetFieldInfoFromLambda<TObj, TReturn>([NotNull] Expression<Func<TObj, TReturn>> fieldExpression) 
        {
            var fieldInfo = GetMemberInfoFromLambda(fieldExpression) as FieldInfo;
            if (fieldInfo == null)
                throw new ArgumentException(String.Format("Expression '{0}' refers to a property, not a field.", fieldExpression));

            return fieldInfo;
        }
    }
}