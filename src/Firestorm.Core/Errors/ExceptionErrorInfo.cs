using System;
using System.Collections.Generic;

namespace Firestorm
{
    /// <summary>
    /// Wraps an <see cref="Exception"/> to give the default error details.
    /// </summary>
    public class ExceptionErrorInfo : ErrorInfo
    {
        public ExceptionErrorInfo(Exception exception)
        {
            ErrorStatus = ExceptionErrorUtilities.GetExceptionStatus(exception);
            ErrorType = ExceptionErrorUtilities.GetExceptionType(exception);
            ErrorDescription = exception.Message;

            StackTrace = exception.StackTrace;
            InnerDescriptions = exception.InnerException != null ? IterateInnerExceptionMessages(exception) : null;
        }

        public string StackTrace { get; private set; }

        public IEnumerable<string> InnerDescriptions { get; private set; }

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