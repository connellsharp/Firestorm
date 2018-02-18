using System.Net;
using Firestorm.Core.Web;

namespace Firestorm.Client
{
    public class ClientRestApiException : RestApiException
    {
        private readonly RestItemData _errorData;

        public ClientRestApiException(HttpStatusCode responseStatusCode, RestItemData errorData) 
            : base((ErrorStatus)(int)responseStatusCode, errorData["error_description"].ToString())
        {
            _errorData = errorData;
        }

        public override string ErrorType
        {
            get { return _errorData["error"].ToString(); }
        }
    }
}