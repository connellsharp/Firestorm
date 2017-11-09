using System;

namespace Firestorm.Engine.Queryable
{
    internal class FieldCannotFilterException : RestApiException
    {
        internal FieldCannotFilterException(string fieldName, Exception innerException)
            : base(ErrorStatus.BadRequest, string.Format("Cannot filter by the '{0}' field.", fieldName), innerException)
        {
        }
    }
}