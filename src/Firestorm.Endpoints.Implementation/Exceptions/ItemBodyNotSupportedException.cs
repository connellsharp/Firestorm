namespace Firestorm.Endpoints
{
    internal class ItemBodyNotSupportedException : RestApiException
    {
        internal ItemBodyNotSupportedException(ResourceType requestedBodyType)
            : base(ErrorStatus.BadRequest, "Request body type " + requestedBodyType.ToString().ToLowerInvariant() + " is not supported for this endpoint.")
        { }
    }
}