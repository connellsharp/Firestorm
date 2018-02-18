using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Firestorm
{
    /// <summary>
    /// Wraps an <see cref="Exception"/> to give the default error details.
    /// </summary>
    public class ExceptionErrorInfo : ErrorInfo
    {
        private readonly Exception _exception;

        public ExceptionErrorInfo(Exception exception)
        {
            _exception = exception;
            ErrorStatus = ExceptionErrorUtilities.GetExceptionStatus(exception);
            ErrorType = ExceptionErrorUtilities.GetExceptionType(exception);
            ErrorDescription = exception.Message;
        }

        [NotNull]
        public IEnumerable<ErrorDeveloperInfo> GetDeveloperInfo()
        {
            Exception exception = _exception;

            do
            {
                yield return new ErrorDeveloperInfo(exception);
                exception = exception.InnerException;
            }
            while (exception != null);
        }
    }

    public class ErrorDeveloperInfo
    {
        internal ErrorDeveloperInfo(Exception exception)
        {
            Message = exception.Message;
            StackTrace = exception.StackTrace;
        }

        public string Message { get; }

        public string StackTrace { get; }
    }
}