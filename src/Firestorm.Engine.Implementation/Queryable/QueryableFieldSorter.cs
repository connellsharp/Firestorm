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
        private readonly IEnumerable<SortInstruction> _instructions;

        internal QueryableFieldSorter(IFieldProvider<TItem> fieldProvider, [CanBeNull] IEnumerable<SortInstruction> instructions)
        {
            _fieldProvider = fieldProvider;
            _instructions = instructions;
        }
        
        public IQueryable<TItem> ApplySortOrder(IQueryable<TItem> items)
        {
            if (_instructions == null)
                return items;

            bool doneOnce = false;

            foreach (SortInstruction sortIntruction in _instructions)
            {
                Debug.Assert(sortIntruction != null);

                LambdaExpression getterLambda = GetOrderSelectorExpression(sortIntruction);
                items = ExpressionTreeHelpers.GetOrderByExpressionQuery(items, getterLambda, sortIntruction.Direction, doneOnce);

                doneOnce = true;
            }

            return items;
        }

        private LambdaExpression GetOrderSelectorExpression(SortInstruction sortInstruction)
        {
            if (!_fieldProvider.FieldExists(sortInstruction.FieldName))
                throw new FieldNotFoundException(sortInstruction.FieldName, false);

            IFieldReader<TItem> fieldReader = _fieldProvider.GetReader(sortInstruction.FieldName);
            if (fieldReader == null)
                throw new FieldOperationNotAllowedException(sortInstruction.FieldName, FieldOperationNotAllowedException.Operation.Read);

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
                throw new FieldCannotFilterException(sortInstruction.FieldName, ex);
            }
        }
    }
}