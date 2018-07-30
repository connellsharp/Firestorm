using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Reflectious;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubItemFieldReader<TItem, TNav> : IFieldReader<TItem>
        where TItem : class
        where TNav : class
    {
        private readonly Expression<Func<TItem, TNav>> _navigationExpression;
        private readonly IEngineSubContext<TNav> _engineSubContext;

        public SubItemFieldReader(Expression<Func<TItem, TNav>> navigationExpression, IEngineSubContext<TNav> engineSubContext)
        {
            _navigationExpression = navigationExpression;
            _engineSubContext = engineSubContext;
            Replacer = new SubItemReplacer<TItem, TNav>(engineSubContext, q => q.Select(navigationExpression));
        }

        public IFieldValueReplacer<TItem> Replacer { get; }
        
        public Type FieldType => typeof(object);

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            var visitedNavigationExpr =
                (LambdaExpression) new ParameterReplacerVisitor(_navigationExpression.Parameters[0], itemPram).Visit(_navigationExpression);

            LambdaExpression memberInitLambda = SubUtilities.GetMemberInitLambda(_engineSubContext.Fields);

            return visitedNavigationExpr.Chain(memberInitLambda).Body;
        }
    }
}