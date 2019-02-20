using System;
using System.Linq;

namespace Firestorm.Endpoints.Query
{
    internal class QueryFilterInstruction : FilterInstruction
    {
        private static readonly char[] OPERATOR_CHARS = new[] { '=', '<', '>', '!', '*', '^', '$' }; // TODO: move to configuration?
        private readonly QueryStringConfiguration _configuration;

        internal QueryFilterInstruction(QueryStringConfiguration configuration, string instructionString)
        {
            _configuration = configuration;
            if (string.IsNullOrEmpty(instructionString))
                throw new ArgumentNullException(nameof(instructionString));

            int operatorIndex = instructionString.IndexOfAny(OPERATOR_CHARS);
            if (operatorIndex == -1)
                throw new ArgumentException("Instruction string does not contain a comparison operator.", nameof(instructionString));

            FieldName = instructionString.Substring(0, operatorIndex);

            string operatorStr = string.Concat(instructionString.Substring(operatorIndex).TakeWhile(c => OPERATOR_CHARS.Contains(c)));
            Operator = ParseComparisonOperator(operatorStr);

            ValueString = instructionString.Substring(operatorIndex + operatorStr.Length);
        }

        private FilterComparisonOperator ParseComparisonOperator(string operatorStr)
        {
            if(!_configuration.WhereFilterComparisonOperators.ContainsKey(operatorStr))
                throw new FormatException("Filter instruction comparison operator is not valid.");

            return _configuration.WhereFilterComparisonOperators[operatorStr];
        }
    }
}