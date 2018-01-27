using System;

namespace Firestorm
{
    internal static class ExceptionErrorUtilities
    {
        internal static ErrorStatus GetExceptionStatus(Exception exception)
        {
            if (exception is RestApiException restApiException)
                return restApiException.ErrorStatus;

            if (exception is NotImplementedException)
                return ErrorStatus.NotImplemented;

            return ErrorStatus.InternalServerError;
        }

        internal static string GetExceptionType(Exception exception)
        {
            if (exception is RestApiException restApiException)
                return restApiException.ErrorType;
            
            return GetTypeString(exception.GetType());
        }

        internal static string GetTypeString(Type type)
        {
            string exceptionTypeName = type.Name.TrimEnd("Exception");
            return exceptionTypeName.SeparateCamelCase("_", true);
        }
    }
}