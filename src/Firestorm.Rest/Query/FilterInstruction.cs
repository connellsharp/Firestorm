namespace Firestorm
{
    public class FilterInstruction
    {
        protected FilterInstruction()
        {
        }

        public FilterInstruction(string fieldName, FilterComparisonOperator comparisonOperator, string valueString)
        {
            FieldName = fieldName;
            Operator = comparisonOperator;
            ValueString = valueString;
        }

        public string FieldName { get; protected set; }

        public FilterComparisonOperator Operator { get; protected set; }

        public string ValueString { get; protected set; }
    }

    public enum FilterComparisonOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEquals,
        LessThan,
        LessThanOrEquals,
        Contains,
        StartsWith,
        EndsWith,
        IsIn
    }
}