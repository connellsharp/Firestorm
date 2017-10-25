using Firestorm.Core;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class SuccessBooleanResponseContentGenerator : IResponseContentGenerator
    {
        public bool WrapResourceObject { get; set; }

        public object GetFromResource(ResourceBody resource)
        {
            if (WrapResourceObject)
            {
                return new
                {
                    success = true,
                    resource = resource.GetObject()
                };
            }
            else
            {
                return resource.GetObject();
            }
        }

        public object GetFromAcknowledgment(Acknowledgment acknowledgment)
        {
            var createdItemAcknowledgment = acknowledgment as CreatedItemAcknowledgment;
            if (createdItemAcknowledgment != null)
            {
                return new
                {
                    success = true,
                    reference = createdItemAcknowledgment.NewIdentifier
                };
            }
            else
            {
                return new
                {
                    success = true
                };
            }
        }

        public object GetFromError(ErrorInfo error)
        {
            return new
            {
                success = false,
                error = error.ErrorType,
                error_description = error.ErrorDescription,
            };
        }

        public object GetFromOptions(Options options)
        {
            throw new System.NotImplementedException();
        }
    }
}