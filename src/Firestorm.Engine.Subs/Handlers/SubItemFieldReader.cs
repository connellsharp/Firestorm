using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubItemFieldReader<TItem, TNav> : SubItemFieldHandlerBase<TItem, TNav>, IFieldReader<TItem>
        where TItem : class
        where TNav : class
    {
        public SubItemFieldReader(Expression<Func<TItem, TNav>> navigationExpression, IEngineSubContext<TNav> engineSubContext)
            : base(navigationExpression, engineSubContext)
        { }

        public Type FieldType => typeof(object);

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            var visitedNavigationExpr = (LambdaExpression) new ParameterReplacerVisitor(NavigationExpression.Parameters[0], itemPram).Visit(NavigationExpression);

            LambdaExpression memberInitLambda = SubUtilities.GetMemberInitLambda(EngineSubContext.FieldProvider);

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