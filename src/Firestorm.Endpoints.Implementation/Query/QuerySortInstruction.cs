using System;

namespace Firestorm.Endpoints.Query
{
    internal class QuerySortInstruction : SortInstruction
    {
        private readonly QueryStringConfiguration _configuration;

        /// <param name="configuration"></param>
        /// <param name="instructionString">The sort instruction string e.g. "name+asc".</param>
        public QuerySortInstruction(QueryStringConfiguration configuration, string instructionString)
        {
            _configuration = configuration;

            string[] split = instructionString.Split(configuration.SortModifierDelimiters);
            if (split.Length > 2)
                throw new FormatException("The clause '" + instructionString + "' contains more than one modifier delimiter character.");

            FieldName = split[0];

            Direction = split.Length == 1 ? SortDirection.Unspecified : ParseSortDirection(split[1]);
        }

        public QuerySortInstruction(QueryStringConfiguration configuration, string fieldName, string sortDirection)
        {
            _configuration = configuration;
            FieldName = fieldName;
            Direction = ParseSortDirection(sortDirection);
        }

        private SortDirection ParseSortDirection(string directionStr)
        {
            if (!_configuration.SortDirectionModifiers.ContainsKey(directionStr))
                throw new FormatException("The string '" + directionStr + "' is not a valid sort order.");

            return _configuration.SortDirectionModifiers[directionStr];
        }
    }
}