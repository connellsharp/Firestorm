namespace Firestorm.Engine
{
    public class MultipleItemsMatchedReferenceException : RestApiException
    {
        internal MultipleItemsMatchedReferenceException()
            : base(ErrorStatus.InternalServerError, "More than one item matches the identifier for this collection.")
        {
        }
    }
}