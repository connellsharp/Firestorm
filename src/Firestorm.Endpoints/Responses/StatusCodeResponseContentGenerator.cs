using Firestorm.Core;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class StatusCodeResponseContentGenerator : IResponseContentGenerator
    {
        public bool WrapResourceObject { get; set; }

        public object GetFromResource(ResourceBody resource)
        {
            if (WrapResourceObject)
            {
                return new
                {
                    status = "ok",
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
            if (acknowledgment is CreatedItemAcknowledgment createdItemAcknowledgment)
            {
                return new
                {
                    status = "created",
                    identifier = createdItemAcknowledgment.NewIdentifier
                };
            }
            else
            {
                return new
                {
                    status = "ok"
                };
            }
        }

        public object GetFromError(ErrorInfo error, bool showDeveloperDetails)
        {
            var data = new RestItemData();
            string status = error.ErrorStatus.ToString().SeparateCamelCase("_", true);
            data.Add("status", status);
            ErrorRestItemUtility.AddErrorData(data, error, showDeveloperDetails);
            return data;
        }

        public object GetFromOptions(Options options)
        {
            throw new System.NotImplementedException();
        }
    }
}