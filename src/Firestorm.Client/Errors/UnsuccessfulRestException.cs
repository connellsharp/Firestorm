namespace Firestorm.Client
{
    public class UnsuccessfulRestException : RestApiException
    {
        public UnsuccessfulRestException(ErrorStatus errorStatus, SuccessOrFailModel successOrFailModel)
            : this(errorStatus, successOrFailModel.ErrorCode, successOrFailModel.ErrorDescription)
        {
        }

        public UnsuccessfulRestException(ErrorStatus errorStatus, string errorCode, string errorDescription)
            : base(errorStatus, string.Format("An error occured on the REST API with code '{0}': {1}", errorCode, errorDescription))
        {
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }

        public string ErrorCode { get; private set; }

        public string ErrorDescription { get; private set; }
    }
}