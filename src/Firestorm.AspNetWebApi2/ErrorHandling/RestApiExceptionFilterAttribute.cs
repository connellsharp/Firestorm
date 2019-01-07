using System;
using System.Net.Http;
using System.Web.Http.Filters;
using Firestorm.Endpoints;

namespace Firestorm.AspNetWebApi2.ErrorHandling
{
    public class RestApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var controller = context.ActionContext.ControllerContext.Controller as FirestormController;
            if (controller == null)
                throw new ArgumentException("RestApiExceptionFilterAttribute should only be applied to FirestormController.");
            
            var exceptionInfo = new ExceptionErrorInfo(context.Exception);
            
            controller.ResponseBuilder.AddError(exceptionInfo);

            context.Response = context.Request.CreateResponse(controller.Response.StatusCode, controller.Response.GetFullBody());
        }
    }
}