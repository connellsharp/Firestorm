using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm.Client
{
    /// <summary>
    /// Capable of serialising queries that are deserialised at the other end using a <see cref="Endpoints.QueryStringCollectionQuery"/>.
    /// </summary>
    public class CollectionQueryStringBuilder
    {
        private readonly CollectionQueryStringConfiguration _configuration;

        public CollectionQueryStringBuilder([NotNull] CollectionQueryStringConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BuildString(IRestCollectionQuery query)
        {
            var builder = new QueryStringBuilder();

            if (query.SelectFields != null)
                AppendSelector(builder, query.SelectFields);

            if (query.FilterInstructions != null)
                AppendFilters(builder, query.FilterInstructions);

            if (query.SortIntructions != null)
                AppendSortOrder(builder, query.SortIntructions);

            // TODO page size

            return builder.ToString();
        }

        private void AppendSelector(QueryStringBuilder builder, IEnumerable<string> selectFields)
        {
            string selectString = string.Join(_configuration.SelectFieldDelimiter.ToString(), selectFields.Select(delegate(string field)
            {
                if(field.Contains(_configuration.SelectFieldDelimiter))
                    throw new ArgumentException("Cannot build fields query value that contain a delimiter character.", nameof(selectFields));

                return field;
            }));

            builder.AppendPair(_configuration.SelectFieldQueryKey, selectString);
        }

        private void AppendFilters(QueryStringBuilder builder, IEnumerable<FilterInstruction> filterInstructions)
        {
            foreach (FilterInstruction instruction in filterInstructions)
            {
                if (_configuration.SpecialFilterKeysEnabled)
                {
                    builder.AppendPair(instruction.FieldName, _configuration.WhereFilterComparisonOperators[instruction.Operator], instruction.ValueString);
                }
                else
                {
                    builder.AppendPair(_configuration.WhereFilterQueryKey,
                        instruction.FieldName + _configuration.WhereFilterComparisonOperators[instruction.Operator] + instruction.ValueString);
                }
            }
        }

        private void AppendSortOrder(QueryStringBuilder builder, IEnumerable<SortIntruction> sortInstructions)
        {
            string sortString = string.Join(_configuration.SortInstructionDelimiter.ToString(), sortInstructions.Select(delegate (SortIntruction instruction)
            {
                if (instruction.FieldName.Contains(_configuration.SelectFieldDelimiter) || instruction.FieldName.Contains(_configuration.SortModifierDelimiter))
                    throw new ArgumentException("Cannot build sort order query value that contain a delimiter character.", nameof(sortInstructions));
                
                string sortModifierStr = _configuration.SortDirectionModifiers[instruction.Direction];

                if (string.IsNullOrEmpty(sortModifierStr))
                    return instruction.FieldName;

                return instruction.FieldName + _configuration.SortModifierDelimiter + sortModifierStr;
            }));

            builder.AppendPair(_configuration.SortOrderQueryKey, sortString);
        }
    }
}