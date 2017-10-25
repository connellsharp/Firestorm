using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Firestorm.Core;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Query
{
    /// <summary>
    /// Capable of deserialising queries that could've been serialised by the client using a <see cref="CollectionQueryStringBuilder"/>.
    /// </summary>
    public class QueryStringCollectionQuery : IRestCollectionQuery
    {
        private readonly CollectionQueryStringConfiguration _configuration;

        private List<string> _selectFields;
        private List<FilterInstruction> _filterInstructions;
        private List<SortIntruction> _sortIntructions;

        public QueryStringCollectionQuery([NotNull] CollectionQueryStringConfiguration configuration, [CanBeNull] string query)
        {
            _configuration = configuration;

            if (string.IsNullOrEmpty(query)) return;

            query = query.TrimStart("?");

            string[] querySplit = query.Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string queryPart in querySplit)
            {
                string[] equalSplit = queryPart.Split(new[] { '=' }, 2);
                if (TryAddValue(UrlDecode(equalSplit[0]), () => UrlDecode(equalSplit[1])))
                    continue;

                if (_configuration.SpecialFilterKeysEnabled)
                {
                    var filter = new QueryFilterInstruction(_configuration, queryPart);
                    AddToList(ref _filterInstructions, filter);
                }
            }
        }

        public QueryStringCollectionQuery([NotNull] CollectionQueryStringConfiguration configuration, [NotNull] NameValueCollection query)
        {
            _configuration = configuration;

            foreach (string key in query.AllKeys)
            {
                if (TryAddValue(key, () => query[key]))
                    continue;

                if (_configuration.SpecialFilterKeysEnabled)
                {
                    string fullString = key + "=" + query[key]; // TODO well this is horrible...
                    var filter = new QueryFilterInstruction(_configuration, fullString);
                    AddToList(ref _filterInstructions, filter);
                }
            }
        }

        private static string UrlDecode(string encodedString)
        {
            return Uri.UnescapeDataString(encodedString);
        }

        private bool TryAddValue(string key, Func<string> getValue)
        {
            if (_configuration.SelectFieldQueryKeys.Contains(key))
            {
                if (_selectFields == null)
                    _selectFields = new List<string>();

                string[] fieldNames = getValue().Split(_configuration.SelectFieldDelimiters);
                _selectFields.AddRange(fieldNames);
                return true;
            }

            if (_configuration.WhereFilterQueryKeys.Contains(key))
            {
                // TODO what about separators like ?where=start>2016-01-01,end<2017-01-01 ?
                var filter = new QueryFilterInstruction(_configuration, getValue());
                AddToList(ref _filterInstructions, filter);
                return true;
            }

            if (_configuration.SortOrderQueryKeys.Contains(key))
            {
                string[] sortStrings = getValue().Split(_configuration.SortInstructionDelimiters);
                foreach (string sortString in sortStrings)
                {
                    var sort = new QuerySortInstruction(_configuration, sortString);
                    AddToList(ref _sortIntructions, sort);
                }
                return true;
            }

            return false;
        }

        private static void AddToList<T>(ref List<T> list, T item)
        {
            if(list == null)
                list = new List<T>();

            list.Add(item);
        }

        public IEnumerable<string> SelectFields
        {
            get { return _selectFields; }
        }

        public IEnumerable<FilterInstruction> FilterInstructions
        {
            get { return _filterInstructions; }
        }

        public IEnumerable<SortIntruction> SortIntructions
        {
            get { return _sortIntructions; }
        }

        public int PageSize
        {
            get { return _configuration.FixedPageSize; }
        }
    }
}