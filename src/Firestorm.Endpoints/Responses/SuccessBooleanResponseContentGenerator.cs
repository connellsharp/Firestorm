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
                    identifier = createdItemAcknowledgment.NewIdentifier
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

        public object GetFromError(ErrorInfo error, bool showDeveloperDetails)
        {
            var data = new RestItemData();
            data.Add("success", false);
            ErrorRestItemUtility.AddErrorData(data, error, showDeveloperDetails);
            return data;
        }

        public object GetFromOptions(Options options)
        {
            throw new System.NotImplementedException();
        }
    }
}