using System;

namespace Firestorm
{
    public class RestApiException : Exception
    {
        public RestApiException(ErrorStatus errorStatus, Exception innerException)
            : base(innerException.Message, innerException)
        {
            ErrorStatus = errorStatus;
        }

        public RestApiException(ErrorStatus errorStatus, string message)
            : base(message)
        {
            ErrorStatus = errorStatus;
        }

        public RestApiException(ErrorStatus errorStatus, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorStatus = errorStatus;
        }

        public ErrorStatus ErrorStatus { get; }

        public virtual string ErrorType { get; } = null;
    }
}