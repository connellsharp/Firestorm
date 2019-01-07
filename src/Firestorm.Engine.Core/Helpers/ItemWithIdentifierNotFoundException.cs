namespace Firestorm.Engine
{
    public class ItemWithIdentifierNotFoundException : RestApiException
    {
        internal ItemWithIdentifierNotFoundException()
            : base(ErrorStatus.NotFound, "No item exists in this collection with this identifier.")
        {
        }
    }
}