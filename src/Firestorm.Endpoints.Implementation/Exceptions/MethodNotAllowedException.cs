namespace Firestorm.Endpoints
{
    internal class MethodNotAllowedException : RestApiException
    {
        internal MethodNotAllowedException(string message)
            : base(ErrorStatus.MethodNotAllowed, message)
        {
        }
    }
}