namespace Firestorm.Client.Content
{
    internal class ExceptionResponse
    {
        public string ErrorDescription { get; set; }
        
        public string Error { get; set; }
        
        public ExceptionDeveloperInfo[] DeveloperInfo { get; set; }
    }

    internal class ExceptionDeveloperInfo
    {
        public string Message { get; set; }

        public string[] StackTrace { get; set; }
    }
}