namespace Firestorm
{
    public class SortInstruction
    {
        protected SortInstruction()
        {
        }

        public SortInstruction(string fieldName, SortDirection sortDirection)
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