namespace Firestorm.Engine.Queryable
{
    internal class FieldOperationNotAllowedException : RestApiException
    {
        internal FieldOperationNotAllowedException(string fieldName, FieldOperation operation)
            : base(ErrorStatus.Forbidden, GetMessage(fieldName, operation))
        {
        }

        private static string GetMessage(string fieldName, FieldOperation operation)
        {
            return "The '" + fieldName + "' field does not allow the "+ operation.ToString().ToLowerInvariant() + " operation.";
        }
    }

    internal enum FieldOperation
    {
        Read,
        Write,
        Sort,
        Filter
    }
}