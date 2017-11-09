using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable.Helpers;
using Firestorm.Stems.Fuel.Resolving;

namespace Firestorm.Stems.Fuel.Substems
{
    internal static class SubstemUtilities
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

        internal static StemEngineSubContext<TNav> StemEngineContextFields<TItem, TNav, TSubstem>(Stem<TItem> stem)
            where TItem : class
            where TNav : class
            where TSubstem : Stem<TNav>
        {
            var autoActivator = new AutoActivator(stem.Configuration.DependencyResolver);
            var substem = autoActivator.CreateInstance<TSubstem>();
            substem.SetParent(stem);

            return new StemEngineSubContext<TNav>(substem);
        }
    }
}