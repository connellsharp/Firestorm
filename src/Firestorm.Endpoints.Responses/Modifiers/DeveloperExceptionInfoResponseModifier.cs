using System;
using System.Collections.Generic;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    internal class DeveloperExceptionInfoResponseModifier : IResponseModifier
    {
        public void AddResource(Response response, ResourceBody resourceBody)
        {
        }

        public void AddAcknowledgment(Response response, Acknowledgment acknowledgment)
        {
        }

        public void AddError(Response response, ErrorInfo error)
        {
            if (error is ExceptionErrorInfo exceptionInfo)
            {
                response.ExtraBody.Add("InnerDescriptions", exceptionInfo.InnerDescriptions);
                response.ExtraBody.Add("StackTrace", exceptionInfo.StackTrace.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        public void AddMultiFeedback(Response response, IEnumerable<Feedback> feedbackItems)
        {
        }

        public void AddOptions(Response response, Options options)
        {
        }
    }
}