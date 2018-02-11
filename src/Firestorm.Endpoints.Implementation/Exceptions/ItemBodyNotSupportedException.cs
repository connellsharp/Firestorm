using Firestorm.Core;

namespace Firestorm.Endpoints
{
    internal class ItemBodyNotSupportedException : RestApiException
    {
        public ItemBodyNotSupportedException(ResourceType requestedBodyType)
            : base(ErrorStatus.BadRequest, "Request body type " + requestedBodyType.ToString().ToLowerInvariant() + " is not supported for this endpoint.")
        { }
    }
}