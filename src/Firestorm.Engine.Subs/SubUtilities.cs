using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable;

namespace Firestorm.Stems.Fuel.Substems
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

            var memberInitLambda = Expression.Lambda(memberInitExpr, navigationParam);
            return memberInitLambda;
        }
    }
}