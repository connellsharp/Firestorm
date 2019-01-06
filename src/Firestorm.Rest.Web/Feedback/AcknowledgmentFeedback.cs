namespace Firestorm.Rest.Web
{
    public class AcknowledgmentFeedback : Feedback
    {
        public AcknowledgmentFeedback(Acknowledgment acknowledgment)
        {
            Acknowledgment = acknowledgment;
        }

        public Acknowledgment Acknowledgment { get; }

        public override FeedbackType Type { get; } = FeedbackType.Acknowledgment;

        public override object GetObject() => Acknowledgment;
    }
}