using System.Collections.Generic;

namespace Firestorm.Core.Web
{
    public class MultiFeedback : Feedback
    {
        public MultiFeedback(IEnumerable<Feedback> feedbackItems)
        {
            FeedbackItems = feedbackItems;
        }

        public IEnumerable<Feedback> FeedbackItems { get; }

        public override FeedbackType Type { get; } = FeedbackType.MultiResponse;

        public override object GetObject() => FeedbackItems;
    }
}