using System;
using System.Collections.Generic;

namespace Firestorm.Core.Web
{
    public class ExceptionErrorInfo : ErrorInfo
    {
        public ExceptionErrorInfo(Exception exception)
            : base(GetErrorStatus(exception), GetExceptionTypeText(exception.GetType()), exception.Message)
        {
            StackTrace = exception.StackTrace;
            InnerDescriptions = exception.InnerException != null ? IterateInnerExceptionMessages(exception) : null;
        }

        private static ErrorStatus GetErrorStatus(Exception exception)
        {
            var restApiException = exception as RestApiException;
            if (restApiException != null)
                return restApiException.ErrorStatus;

            return exception is NotImplementedException ? ErrorStatus.NotImplemented : ErrorStatus.InternalServerError;
        }
        
        public IEnumerable<string> InnerDescriptions { get; private set; }

        public string StackTrace { get; private set; }

        private static string GetExceptionTypeText(Type exceptionType)
        {
            string exceptionTypeName = exceptionType.Name.TrimEnd("Exception");
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