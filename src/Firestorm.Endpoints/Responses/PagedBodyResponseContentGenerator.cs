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
                    page = pagedResource.PageDetails
                };
            }

            return resource.GetObject();
        }

        public object GetFromAcknowledgment(Acknowledgment acknowledgment)
        {
            return null;
        }

        public bool ShowDeveloperErrors { get; set; }

        public object GetFromError(ErrorInfo error)
        {
            return ErrorRestItemUtility.GetErrorData(error, ShowDeveloperErrors);
        }

        public object GetFromOptions(Options options)
        {
            throw new System.NotImplementedException();
        }
    }
}