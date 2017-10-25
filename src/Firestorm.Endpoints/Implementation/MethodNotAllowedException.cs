using System;
using Firestorm.Core;

namespace Firestorm.Endpoints
{
    public class MethodNotAllowedException : RestApiException
    {
        public MethodNotAllowedException(string message)
            : base(ErrorStatus.MethodNotAllowed, message)
        {
        }
    }
}