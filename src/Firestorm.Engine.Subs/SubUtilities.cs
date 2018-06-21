using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable;

namespace Firestorm.Engine.Subs
{
    internal static class SubUtilities
    {
        internal static LambdaExpression GetMemberInitLambda<TNav>(IFieldProvider<TNav> fieldProvider)
            where TNav : class
        {
            ParameterExpression navigationParam = Expression.Parameter(typeof(TNav), "n");

            Type dynamicType = FieldProviderUtility.GetDynamicType(fieldProvider);
            var initExpressionBuilder = new MemberInitExpressionBuilder(dynamicType);
            MemberInitExpression memberInitExpr = initExpressionBuilder.Build(navigationParam, fieldProvider);

            //return Expression.Lambda(memberInitExpr, navigationParam);
            return NullConditionalLambda(memberInitExpr, navigationParam);
        }

        private static LambdaExpression NullConditionalLambda(Expression body, ParameterExpression parameter)
        {
            BinaryExpression nullCheck = Expression.Equal(parameter, Expression.Constant(null, parameter.Type));
            var nullCondition = Expression.Condition(nullCheck, Expression.Constant(null, body.Type), body);

            var nullCheckInitLambda = Expression.Lambda(nullCondition, parameter);
            return nullCheckInitLambda;
        }
    }
}