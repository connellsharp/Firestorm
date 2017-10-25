using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;

namespace Firestorm.Endpoints.Responses
{
    public class DirectResponseContentGenerator : IResponseContentGenerator
    {
        public object GetFromResource(ResourceBody resource)
        {
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