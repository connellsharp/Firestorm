namespace Firestorm.Engine.Fields
{
    internal class FieldOperationNotAllowedException : RestApiException
    {
        internal FieldOperationNotAllowedException(string fieldName, Operation operation)
            : base(ErrorStatus.Forbidden, "The '" + fieldName + "' field does not allow the "+ operation + " operation.")
        {
        }

        internal enum Operation
        {
            Read,
            Write
        }
    }
}