using System;

namespace Firestorm.Engine
{
    public abstract class NotAuthorizedException : RestApiException
    {
        protected NotAuthorizedException(AuthorizableVerb verb, string thing)
            : base(ErrorStatus.Forbidden, string.Format("User is not authorised to {0} {1}.", verb.ToString().ToLower(), thing))
        { }
    }

    public enum AuthorizableVerb
    {
        Get,
        Add,
        Edit,
        Delete
    }

    public class NotAuthorizedForItemException : NotAuthorizedException
    {
        internal NotAuthorizedForItemException(AuthorizableVerb verb)
            : base(verb, "this item")
        { }
    }

    public class NotAuthorizedForFieldException : NotAuthorizedException
    {
        internal NotAuthorizedForFieldException(AuthorizableVerb verb, string fieldName)
            : base(verb, "the '" + fieldName + "' field")
        { }
    }
}