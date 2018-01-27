using System;

namespace Firestorm
{
    /// <summary>
    /// An exception containing an error message to respond with in an REST API.
    /// Messages are to be read by the consumer of the REST API.
    /// </summary>
    public class RestApiException : Exception
    {
        public RestApiException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorStatus = ExceptionErrorUtilities.GetExceptionStatus(innerException);
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

        public virtual string ErrorType
        {
            get { return ExceptionErrorUtilities.GetExceptionTypeString(GetType()); }
        }
    }
}