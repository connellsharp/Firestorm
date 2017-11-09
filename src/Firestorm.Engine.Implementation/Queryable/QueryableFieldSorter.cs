using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Queryable
{
    public class QueryableFieldSorter<TItem>
        where TItem : class
    {
        private readonly IFieldProvider<TItem> _fieldProvider;
        private readonly IEnumerable<SortIntruction> _instructions;

        internal QueryableFieldSorter(IFieldProvider<TItem> fieldProvider, [CanBeNull] IEnumerable<SortIntruction> instructions)
        {
            _fieldProvider = fieldProvider;
            _instructions = instructions;
        }
        
        public IQueryable<TItem> ApplySortOrder(IQueryable<TItem> items)
        {
            if (_instructions == null)
                return items;

            bool doneOnce = false;

            foreach (SortIntruction sortIntruction in _instructions)
            {
                Debug.Assert(sortIntruction != null);

                LambdaExpression getterLambda = GetOrderSelectorExpression(sortIntruction);
                items = ExpressionTreeHelpers.GetOrderByExpressionQuery(items, getterLambda, sortIntruction.Direction, doneOnce);

                doneOnce = true;
            }

            return items;
        }

        private LambdaExpression GetOrderSelectorExpression(SortIntruction sortIntruction)
        {
            if (!_fieldProvider.FieldExists(sortIntruction.FieldName))
                throw new FieldNotFoundException(sortIntruction.FieldName, false);

            IFieldReader<TItem> fieldReader = _fieldProvider.GetReader(sortIntruction.FieldName);
            if (fieldReader == null)
                throw new FieldOperationNotAllowedException(sortIntruction.FieldName, FieldOperationNotAllowedException.Operation.Read);

            try
            {
                ParameterExpression itemPram = Expression.Parameter(typeof(TItem), "srt");
                LambdaExpression getterLambda = fieldReader.GetSortExpression(itemPram);

                if (getterLambda == null)
                    throw new NullReferenceException("No sort expression was defined for this field.");

                return getterLambda;
            }
            catch (Exception ex)
            {
                throw new FieldCannotFilterException(sortIntruction.FieldName, ex);
            }
        }
    }
}