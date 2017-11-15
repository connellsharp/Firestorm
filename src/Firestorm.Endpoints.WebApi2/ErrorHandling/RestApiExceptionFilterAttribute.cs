using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.WebApi2.ErrorHandling
{
    public class RestApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var controller = context.ActionContext.ControllerContext.Controller as FirestormController;
            if (controller == null)
                throw new ArgumentException("RestApiExceptionFilterAttribute should only be applied to FirestormController.");
            
            IResponseContentGenerator contentGenerator = controller.Config.EndpointConfiguration.ResponseContentGenerator;

            var exceptionInfo = new ExceptionErrorInfo(context.Exception);
            object content = contentGenerator.GetFromError(exceptionInfo, false);

            context.Response = context.Request.CreateResponse((HttpStatusCode) exceptionInfo.ErrorStatus, content);
        }
    }
}