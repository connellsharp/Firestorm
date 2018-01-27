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

        internal static string GetExceptionTypeText(Exception exception)
        {
            if (exception is RestApiException restApiException)
                return restApiException.ErrorType;
            
            return GetExceptionTypeString(exception.GetType());
        }

        internal static string GetExceptionTypeString(Type type)
        {
            string exceptionTypeName = type.Name.TrimEnd("Exception");
            return exceptionTypeName.SeparateCamelCase("_", true);
        }
    }
}