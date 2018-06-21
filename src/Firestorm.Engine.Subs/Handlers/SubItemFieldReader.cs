using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Reflectious;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubItemFieldReader<TItem, TNav> : SubItemFieldHandlerBase<TItem, TNav>, IFieldReader<TItem>
        where TItem : class
        where TNav : class
    {
        public SubItemFieldReader(Expression<Func<TItem, TNav>> navigationExpression,
            IEngineSubContext<TNav> engineSubContext)
            : base(navigationExpression, engineSubContext)
        {
            Replacer = new SubItemReplacer<TItem, TNav>(engineSubContext, q => q.Select(navigationExpression));
        }

        public IFieldValueReplacer<TItem> Replacer { get; }
        
        public Type FieldType => typeof(object);

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            var visitedNavigationExpr =
                (LambdaExpression) new ParameterReplacerVisitor(NavigationExpression.Parameters[0], itemPram).Visit(NavigationExpression);

            LambdaExpression memberInitLambda = SubUtilities.GetMemberInitLambda(EngineSubContext.Fields);

            return visitedNavigationExpr.Chain(memberInitLambda).Body;
        }
    }
}