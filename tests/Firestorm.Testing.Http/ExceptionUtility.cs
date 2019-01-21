using System;

namespace Firestorm.Testing.Http
{
    public static class ExceptionUtility
    {
        public static string GetExceptionJson(Exception ex)
        {
            return "{" +
                   "\"error\": \"critical_unhandled_error\"," +
                   "\"error_description\": \"" + ex.Message + "\"," +
                   "\"developer_info\": [" +
                   "{ \"stack_trace\": [ \"" + ex.StackTrace.Replace("\"", "\\\"").Replace("\\", "\\\\").Replace(Environment.NewLine, "\", \"") + "\" ] }" +
                   "] " +
                   "}";
        }
    }
}