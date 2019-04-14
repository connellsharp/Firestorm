using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;

namespace Firestorm.FluentValidation
{
    public class ValidationRestApiException : RestApiException
    {
        internal ValidationRestApiException(IEnumerable<ValidationFailure> errors)
            : base(ErrorStatus.BadRequest, GetErrorMessage(errors)) // TODO other status codes e.g. 422
        {
        }

        private static string GetErrorMessage(IEnumerable<ValidationFailure> errors)
        {
            var stringBuilder = new StringBuilder("Validation failed: ");

            foreach (ValidationFailure error in errors)
            {
                stringBuilder.AppendLine(error.ErrorMessage);
            }

            return stringBuilder.ToString();
        }
    }
}