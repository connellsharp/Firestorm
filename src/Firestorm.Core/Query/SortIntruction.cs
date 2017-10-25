namespace Firestorm
{
    public class SortIntruction
    {
        protected SortIntruction()
        {
        }

        public SortIntruction(string fieldName, SortDirection sortDirection)
        {
            FieldName = fieldName;
            Direction = sortDirection;
        }

        public string FieldName { get; protected set; }

        public SortDirection Direction { get; protected set; }
    }

    public enum SortDirection
    {
        Unspecified = -1,
        Ascending = 0,
        Descending = 1
    }
}