using System;
using System.Collections.Generic;
using System.Net;
using Firestorm.Client.Content;

namespace Firestorm.Client
{
    public class ClientRestApiException : RestApiException
    {
        private readonly ExceptionResponse _exceptionResponse;

        internal ClientRestApiException(HttpStatusCode responseStatusCode, ExceptionResponse exceptionResponse) 
            : base((ErrorStatus)(int)responseStatusCode, null, GetFakeInnerException(exceptionResponse))
        {
            _exceptionResponse = exceptionResponse;
        }

        private static ClientInnerException GetFakeInnerException(ExceptionResponse exceptionResponse)
        {
            var devInfo = exceptionResponse.DeveloperInfo;
            return new ClientInnerException(devInfo, 0);
        }

        public override string Message => "An error occurred in the API behind this client: " + _exceptionResponse.ErrorDescription;

        public override string ErrorType => _exceptionResponse.Error;

        public class ClientInnerException : Exception
        {
            private readonly ExceptionDeveloperInfo _devInfo;

            internal ClientInnerException(IReadOnlyList<ExceptionDeveloperInfo> devInfoArr, int index)
                : base(null, devInfoArr.Count > index + 1 ? new ClientInnerException(devInfoArr, index + 1) : null)
            {
                _devInfo = devInfoArr[index];
            }

            public override string Message => _devInfo.Message;

            public override string StackTrace => string.Join(Environment.NewLine, _devInfo.StackTrace);
        }
    }
}