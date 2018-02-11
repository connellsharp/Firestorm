using System;

namespace Firestorm.Endpoints.Strategies
{
    internal class MethodNotAllowedException : RestApiException
    {
        internal MethodNotAllowedException(UnsafeMethod method, Type resourceType)
            : base(ErrorStatus.MethodNotAllowed, GetMessage(method, resourceType))
        {
        }

        private static string GetMessage(UnsafeMethod method, Type resourceType)
        {
            string resourceTypeName = resourceType.Name.Replace("IRest", string.Empty).ToLower();
            return "The " + method.ToString().ToUpper() + " method is not allowed on a " + resourceTypeName;
        }
    }
}