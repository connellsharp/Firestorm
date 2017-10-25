using System;
using Firestorm.Core.Web;

namespace Firestorm.Endpoints.Responses
{
    internal static class ErrorRestItemUtility
    {
        public static RestItemData GetErrorData(ErrorInfo error, bool showDeveloperInfo)
        {
            var data = new RestItemData();
            AddErrorData(data, error, showDeveloperInfo);
            return data;
        }

        public static void AddErrorData(RestItemData data, ErrorInfo error, bool showDeveloperInfo)
        {
            data.Add("error", error.ErrorType);
            data.Add("error_description", error.ErrorDescription);

            if (showDeveloperInfo && error is ExceptionErrorInfo exceptionInfo)
            {
                data.Add("inner_descriptions", exceptionInfo.InnerDescriptions);
                data.Add("stack_trace", exceptionInfo.StackTrace.Split(new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            }
        }
    }
}