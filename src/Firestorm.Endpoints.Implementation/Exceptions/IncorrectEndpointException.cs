using Firestorm.Core;

namespace Firestorm.Endpoints
{
    public class IncorrectEndpointException : RestApiException
    {
        public IncorrectEndpointException(string message)
            :base (ErrorStatus.NotFound, message)
        {
        }
    }
}