using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Queryable
{
    public class QueryableFieldFilter<TItem>
        where TItem : class
    {
        private readonly IFieldProvider<TItem> _fieldProvider;
        private readonly IEnumerable<FilterInstruction> _instructions;

        public QueryableFieldFilter(IFieldProvider<TItem> fieldProvider, [CanBeNull] IEnumerable<FilterInstruction> instructions)
        {
            _fieldProvider = fieldProvider;
            _instructions = instructions;
        }

        public IQueryable<TItem> ApplyFilter(IQueryable<TItem> items)
        {
            if (_instructions == null)
                return items;

            foreach (FilterInstruction filterInstruction in _instructions)
            {
                var filterExpr = GetFilterPredicateExpression(filterInstruction);
                items = items.Where(filterExpr);
            }

            return items;
        }

        /// <remarks>
        /// Ideas from https://stackoverflow.com/questions/5094489
        /// </remarks>
        private Expression<Func<TItem, bool>> GetFilterPredicateExpression(FilterInstruction filterInstruction)
        {
            if (!_fieldProvider.FieldExists(filterInstruction.FieldName))
                throw new FieldNotFoundException(filterInstruction.FieldName, false);

            IFieldCollator<TItem> fieldCollator = _fieldProvider.GetCollator(filterInstruction.FieldName);
            if(fieldCollator == null)
                    throw new FieldOperationNotAllowedException(filterInstruction.FieldName, FieldOperation.Filter);

            try
            {
                ParameterExpression itemPram = Expression.Parameter(typeof(TItem), "fltr");

                Expression filterExpression = fieldCollator.GetFilterExpression(itemPram, filterInstruction.Operator, filterInstruction.ValueString);
                if(filterExpression == null)
                    throw new ArgumentNullException(nameof(filterExpression), "No filter expression was defined for this field.");

                Expression<Func<TItem, bool>> predicateLambda = Expression.Lambda<Func<TItem, bool>>(filterExpression, itemPram);
                return predicateLambda;
            }
            catch (Exception ex)
            {
                throw new FieldCannotFilterException(filterInstruction.FieldName, ex);
            }
        }
    }
}