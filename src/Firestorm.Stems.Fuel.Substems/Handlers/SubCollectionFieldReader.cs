using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Engine.Fields;

namespace Firestorm.Stems.Fuel.Substems.Handlers
{
    internal class SubCollectionFieldReader<TItem, TNav> : IFieldReader<TItem>
        where TItem : class
        where TNav : class, new()
    {
        private readonly Expression<Func<TItem, IEnumerable<TNav>>> _navigationExpression;
        private readonly StemEngineSubContext<TNav> _substemSubContext;

        public SubCollectionFieldReader(Expression<Func<TItem, IEnumerable<TNav>>> navigationExpression, StemEngineSubContext<TNav> substemSubContext)
        {
            _navigationExpression = navigationExpression;
            _substemSubContext = substemSubContext;
        }

        public Type FieldType => typeof(object); // TODO maybe enumerable ?

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            var visitedNavigationExpr = (LambdaExpression) new ParameterReplacerVisitor(_navigationExpression.Parameters[0], itemPram).Visit(_navigationExpression);

            LambdaExpression memberInitLambda = SubstemUtilities.GetMemberInitLambda(_substemSubContext.FieldProvider);

            Type dynamicType = memberInitLambda.ReturnType;
            MethodCallExpression selectMethodExpr = Expression.Call(typeof(Enumerable), "Select", new[] { typeof(TNav), dynamicType }, visitedNavigationExpr.Body, memberInitLambda);
            return selectMethodExpr;
        }

        public IFieldValueReplacer<TItem> Replacer { get; } = null;

        public Expression GetFilterExpression(ParameterExpression itemPram, FilterComparisonOperator comparisonOperator, string valueString)
        {
            throw new NotSupportedException("Filtering is not supported for sub collections.");
        }

        public LambdaExpression GetSortExpression(ParameterExpression itemPram)
        {
            throw new NotSupportedException("Sorting is not supported for sub collections.");
        }
    }
}