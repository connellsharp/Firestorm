using System.Collections.Generic;

namespace Firestorm.Endpoints.Configuration
{
    /// <summary>
    /// The configuration used to build the <see cref="QueryStringCollectionQuery"/> from a requested query string.
    /// </summary>
    public class QueryStringConfiguration
    {
        /// <summary>
        /// The querystring keys used to specify which fields to select.
        /// </summary>
        public string[] SelectFieldQueryKeys { get; set; } = new[] { "select", "fields" };
        
        /// <summary>
        /// The delimiter characters used to split a select fields list.
        /// </summary>
        public char[] SelectFieldDelimiters { get; set; } = new[] { ',', ';', '+', ' ' };

        /// <summary>
        /// The querystring keys used to specify filter instructions.
        /// </summary>
        public string[] WhereFilterQueryKeys { get; set; } = new[] { "where", "filter" };

        /// <summary>
        /// A flag to allow unreserved key names to be used directly as filters e.g. ?select=id&amp;name=fred
        /// </summary>
        public bool SpecialFilterKeysEnabled { get; set; } = true;

        /// <summary>
        /// The comparison operator strings.
        /// </summary>
        public Dictionary<string, FilterComparisonOperator> WhereFilterComparisonOperators { get; } = new Dictionary<string, FilterComparisonOperator>
        {
            { "=", FilterComparisonOperator.Equals },
            { "==", FilterComparisonOperator.Equals },
            { "!=", FilterComparisonOperator.NotEquals },
            { "!==", FilterComparisonOperator.NotEquals },
            { "<>", FilterComparisonOperator.NotEquals },
            { ">", FilterComparisonOperator.GreaterThan },
            { ">=", FilterComparisonOperator.GreaterThanOrEquals },
            { "<", FilterComparisonOperator.LessThan },
            { "<=", FilterComparisonOperator.LessThanOrEquals },
            { "*=", FilterComparisonOperator.Contains },
            { "=*", FilterComparisonOperator.IsIn },
            { "^=", FilterComparisonOperator.StartsWith },
            { "$=", FilterComparisonOperator.EndsWith },
        };

        /// <summary>
        /// The querystring keys used to specify the sort instructions.
        /// </summary>
        public string[] SortOrderQueryKeys { get; set; } = new[] { "sort", "order", "orderby" };

        /// <summary>
        /// The delimiters used to split a sort instruction list.
        /// </summary>
        public char[] SortInstructionDelimiters { get; set; } = new[] { ',', ';' };

        /// <summary>
        /// The delimiters used to split an individual sort instruction into its field name and direction modifier.
        /// </summary>
        public char[] SortModifierDelimiters { get; set; } = new[] { '+', ' ' };

        /// <summary>
        /// The sort direction modifier strings.
        /// </summary>
        public Dictionary<string, SortDirection> SortDirectionModifiers { get; } = new Dictionary<string, SortDirection>
        {
            { "asc", SortDirection.Ascending },
            { "ascending", SortDirection.Ascending },
            { "desc", SortDirection.Descending },
            { "descending", SortDirection.Descending },
            { string.Empty, SortDirection.Unspecified },
            //{ null, SortDirection.Unspecified }, // throws exception!
        };

        /// <summary>
        /// The querystring keys used to specify the number of items to return per page.
        /// </summary>
        public string[] PageSizeQueryKeys { get; set; } = new[] { "limit", "take", "size", "per_page" };

        /// <summary>
        /// The querystring keys used to specify how many items to skip to get to the start of this page.
        /// </summary>
        public string[] PageOffsetQueryKeys { get; set; } = new[] { "offset", "skip" };

        /// <summary>
        /// The querystring keys used to specify the page number.
        /// </summary>
        public string[] PageNumberQueryKeys { get; set; } = new[] { "page" };

        /// <summary>
        /// Special page strings that can be translated in page numbers.
        /// </summary>
        public Dictionary<string, int> SpecialPageNumbers { get; } = new Dictionary<string, int>
        {
            { "first", 1 },
            { "start", 1 },
            { "last", -1 },
            { "end", -1 },
        };

        /// <summary>
        /// A prefix that when applied to an identifier in the URL can return an <see cref="IRestDictionary"/> rather than a single item.
        /// </summary>
        public string DictionaryReferencePrefix { get; set; } = "by_";
    }
}