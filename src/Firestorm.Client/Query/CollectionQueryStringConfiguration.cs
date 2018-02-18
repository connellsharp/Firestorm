using System.Collections.Generic;

namespace Firestorm.Client.Query
{
    /// <summary>
    /// The configuration used to build a query string from a <see cref="IRestCollectionQuery"/>.
    /// </summary>
    /// <remarks>
    /// Not to be confused with the class by the same name in the Firestorm.Endpoints library.
    /// That has similar properties but is used to parse the string and therefore sometimes can have multiple options (e.g. "fields" or "select")
    /// </remarks>
    public class CollectionQueryStringConfiguration
    {
        /// <summary>
        /// The querystring key used to specify which fields to select.
        /// </summary>
        public string SelectFieldQueryKey { get; } = "fields";

        /// <summary>
        /// The delimiter character used to split a select fields list.
        /// </summary>
        public char SelectFieldDelimiter { get; } = ',';

        /// <summary>
        /// The querystring key used to specify filter instructions.
        /// </summary>
        public string WhereFilterQueryKey { get; } = "where";

        /// <summary>
        /// A flag to allow unreserved key names to be used directly as filters e.g. ?select=id&amp;name=fred
        /// </summary>
        public bool SpecialFilterKeysEnabled { get; } = true;

        /// <summary>
        /// The comparison operator strings.
        /// </summary>
        public Dictionary<FilterComparisonOperator, string> WhereFilterComparisonOperators { get; } = new Dictionary<FilterComparisonOperator, string>
        {
            { FilterComparisonOperator.Equals, "=" },
            { FilterComparisonOperator.NotEquals, "!=" },
            { FilterComparisonOperator.GreaterThan, ">" },
            { FilterComparisonOperator.GreaterThanOrEquals, ">=" },
            { FilterComparisonOperator.LessThan, "<" },
            { FilterComparisonOperator.LessThanOrEquals, "<=" },
            { FilterComparisonOperator.Contains, "*=" },
            { FilterComparisonOperator.StartsWith, "^=" },
            { FilterComparisonOperator.EndsWith, "$=" },
        };

        /// <summary>
        /// The querystring key used to specify the sort instructions.
        /// </summary>
        public string SortOrderQueryKey { get; } = "sort";

        /// <summary>
        /// The delimiter used to split a sort instruction list.
        /// </summary>
        public char SortInstructionDelimiter { get; } = ',';

        /// <summary>
        /// The delimiter used to split an individual sort instruction into its field name and direction modifier.
        /// </summary>
        public char SortModifierDelimiter { get; } = '+';

        /// <summary>
        /// The sort direction modifier strings.
        /// </summary>
        public Dictionary<SortDirection, string> SortDirectionModifiers { get; } = new Dictionary<SortDirection, string>
        {
            { SortDirection.Ascending, "asc" },
            { SortDirection.Descending, "desc" },
            { SortDirection.Unspecified, string.Empty }
        };

        /// <summary>
        /// The number of items to return in a collection.
        /// </summary>
        public int FixedPageSize { get; } = 100;

        /// <summary>
        /// A prefix that when applied to an identifier in the URL can return an <see cref="IRestDictionary"/> rather than a single item.
        /// </summary>
        public string DictionaryReferencePrefix { get; } = "by_";
    }
}