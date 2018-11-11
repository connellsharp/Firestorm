namespace Firestorm
{
    public class ErrorInfo
    {
        internal ErrorInfo()
        { }

        public ErrorInfo(ErrorStatus errorStatus, string errorType, string errorDescription)
        {
            ErrorStatus = errorStatus;
            ErrorType = errorType;
            ErrorDescription = errorDescription;
        }

        public ErrorStatus ErrorStatus { get; internal set; }

        public string ErrorType { get; internal set; }

        public string ErrorDescription { get; internal set; }
    }
}