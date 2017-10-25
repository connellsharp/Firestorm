namespace Firestorm.Engine.Fields
{
    internal class IdentifierNotFoundException : RestApiException
    {
        internal IdentifierNotFoundException(string identifierName)
            : base(ErrorStatus.NotFound,
                "An identifier with the name '" + identifierName + "' was not found in this item type.")
        {
        }
    }
}