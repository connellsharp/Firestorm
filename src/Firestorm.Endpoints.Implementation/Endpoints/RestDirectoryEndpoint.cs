using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Preconditions;

namespace Firestorm.Endpoints
{
    internal class RestDirectoryEndpoint : IRestEndpoint
    {
        internal RestDirectoryEndpoint(IRestEndpointContext endpointContext, IRestDirectory directory)
        {
            Context = endpointContext;
            Directory = directory;
        }

        private IRestEndpointContext Context { get; }

        private IRestDirectory Directory { get; }

        public IRestEndpoint Next(string resourceName)
        {
            IRestResource resource = Directory.GetChild(resourceName);

            return Endpoint.GetFromResource(Context, resource);
        }

        public async Task<ResourceBody> GetAsync(IRestCollectionQuery query)
        {
            RestDirectoryInfo directory = await Directory.GetInfoAsync();
            return new DirectoryBody(directory);
        }

        public async Task<Options> OptionsAsync()
        {
            var options = new Options();

            RestDirectoryInfo info = await Directory.GetInfoAsync();

            options.SubResources.AddRange(info.Resources);

            return options;
        }

        public Task<Feedback> UnsafeAsync(UnsafeMethod method, ResourceBody body)
        {
            throw new MethodNotAllowedException("Only safe methods are allowed on the directory.");
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return true;
            // TODO: implement precondition checks
        }
    }
}