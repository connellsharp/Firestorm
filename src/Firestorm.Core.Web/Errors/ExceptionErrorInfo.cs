using System;
using System.Collections.Generic;

namespace Firestorm.Core.Web
{
    public class ExceptionErrorInfo : ErrorInfo
    {
        public ExceptionErrorInfo(Exception exception)
            : base(GetErrorStatus(exception), GetExceptionTypeText(exception), exception.Message)
        {
            StackTrace = exception.StackTrace;
            InnerDescriptions = exception.InnerException != null ? IterateInnerExceptionMessages(exception) : null;
        }

        private static ErrorStatus GetErrorStatus(Exception exception)
        {
            if (exception is RestApiException restApiException)
                return restApiException.ErrorStatus;

            return exception is NotImplementedException ? ErrorStatus.NotImplemented : ErrorStatus.InternalServerError;
        }
        
        public IEnumerable<string> InnerDescriptions { get; private set; }

        public string StackTrace { get; private set; }

        private static string GetExceptionTypeText(Exception exception)
        {
            if (exception is RestApiException restApiException && !string.IsNullOrEmpty(restApiException.ErrorType))
                return restApiException.ErrorType;

            // TODO: move lower and not rely on the null check?
            string exceptionTypeName = exception.GetType().Name.TrimEnd("Exception");
            return exceptionTypeName.SeparateCamelCase("_", true);
        }

        private static IEnumerable<string> IterateInnerExceptionMessages(Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                yield return exception.Message;
            }
        }
    }
}