using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class PagedBodyResponseContentGenerator : IResponseContentGenerator
    {
        public object GetFromResource(ResourceBody resource)
        {
            if (resource is IPagedResourceBody pagedResource)
            {
                return new
                {
                    items = resource.GetObject(),
                    page = pagedResource.PageLinks
                };
            }

            return resource.GetObject();
        }

        public object GetFromAcknowledgment(Acknowledgment acknowledgment)
        {
            return null;
        }

        public object GetFromError(ErrorInfo error, bool showDeveloperDetails)
        {
            return ErrorRestItemUtility.GetErrorData(error, showDeveloperDetails);
        }

        public object GetFromOptions(Options options)
        {
            throw new System.NotImplementedException();
        }
    }
}