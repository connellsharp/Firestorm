using System;
using System.Collections.Generic;
using System.Linq;
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
                response.ExtraBody.Add("DeveloperInfo", exceptionInfo.GetDeveloperInfo().Select(info => new
                {
                    Message = info.Message,
                    StackTrace = info.StackTrace?.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                }));
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