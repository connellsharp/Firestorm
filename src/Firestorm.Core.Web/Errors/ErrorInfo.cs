using System.Runtime.Serialization;

namespace Firestorm.Core.Web
{
    public class ErrorInfo
    {
        public ErrorInfo(ErrorStatus errorStatus, string errorType, string errorDescription)
        {
            ErrorType = errorType;
            ErrorDescription = errorDescription;
            ErrorStatus = errorStatus;
        }

        public ErrorStatus ErrorStatus { get; private set; }
        
        public string ErrorType { get; private set; }
        
        public string ErrorDescription { get; private set; }
    }
}