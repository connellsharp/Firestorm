namespace Firestorm.Engine.Queryable
{
    internal class FieldNotFoundException : RestApiException
    {
        internal FieldNotFoundException(string fieldName, bool fromPath)
            : base(fromPath ? ErrorStatus.NotFound : ErrorStatus.BadRequest,
                "A field with the name '" + fieldName + "' was not found in this item type.")
        {
        }
    }
}