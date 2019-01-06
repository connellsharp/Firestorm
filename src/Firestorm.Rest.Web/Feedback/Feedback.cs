namespace Firestorm.Rest.Web
{
    /// <summary>
    /// Can be an acknowledgment, error or multi-status response from an endpoint.
    /// </summary>
    public abstract class Feedback
    {
        internal Feedback()
        { }

        public abstract FeedbackType Type { get; }

        public abstract object GetObject();
    }
}