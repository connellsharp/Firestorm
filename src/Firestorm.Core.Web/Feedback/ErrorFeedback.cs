namespace Firestorm.Core.Web
{
    public class ErrorFeedback : Feedback
    {
        public ErrorFeedback(ErrorInfo error)
        {
            Error = error;
        }

        public ErrorInfo Error { get; }

        public override FeedbackType Type { get; } = FeedbackType.Error;

        public override object GetObject() => Error;
    }
}