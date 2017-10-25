using System;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Engine.Fields;

namespace Firestorm.Stems.Fuel.Substems.Handlers
{
    internal class SubItemFieldReader<TItem, TNav> : SubItemFieldHandler<TItem, TNav>, IFieldReader<TItem>
        where TItem : class
        where TNav : class, new()
    {
        public SubItemFieldReader(Expression<Func<TItem, TNav>> navigationExpression, StemEngineSubContext<TNav> engineSubContext)
            : base(navigationExpression, engineSubContext)
        { }

        public Type FieldType => typeof(object);

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            var visitedNavigationExpr = (LambdaExpression) new ParameterReplacerVisitor(NavigationExpression.Parameters[0], itemPram).Visit(NavigationExpression);

            LambdaExpression memberInitLambda = SubstemUtilities.GetMemberInitLambda(EngineSubContext.FieldProvider);

            return visitedNavigationExpr.Chain(memberInitLambda).Body;
        }

        public IFieldValueReplacer<TItem> Replacer { get; } = null;

        public Expression GetFilterExpression(ParameterExpression itemPram, FilterComparisonOperator comparisonOperator, string valueString)
        {
            throw new NotSupportedException("Filtering is not supported for sub items.");
        }

        public LambdaExpression GetSortExpression(ParameterExpression itemPram)
        {
            throw new NotSupportedException("Sorting is not supported for sub items.");
        }
    }
}