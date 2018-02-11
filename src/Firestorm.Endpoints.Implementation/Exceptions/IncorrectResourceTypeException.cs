using System;

namespace Firestorm.Endpoints
{
    public class IncorrectResourceTypeException : RestApiException
    {
        public IncorrectResourceTypeException(Type resourceType)
            :base (ErrorStatus.InternalServerError, "The resource type '"+ resourceType.Name + "' was not a valid IRestResource type.")
        {
        }
    }
}