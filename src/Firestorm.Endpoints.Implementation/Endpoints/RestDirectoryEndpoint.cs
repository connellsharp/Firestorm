using System.Threading.Tasks;
using Firestorm.Host.Infrastructure;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints
{
    internal class RestDirectoryEndpoint : IRestEndpoint
    {
        internal RestDirectoryEndpoint(IEndpointContext endpointContext, IRestDirectory directory)
        {
            Context = endpointContext;
            Directory = directory;
        }

        private IEndpointContext Context { get; }

        private IRestDirectory Directory { get; }

        public IRestEndpoint Next(INextPath resourceName)
        {
            IRestResource resource = Directory.GetChild(resourceName.GetCoded());
            if (resource == null)
                return null;

            return Context.Configuration.EndpointResolver.GetFromResource(Context, resource);
        }

        public async Task<ResourceBody> GetAsync(IRestCollectionQuery query)
        {
            RestDirectoryInfo directory = await Directory.GetInfoAsync();
            return new DirectoryBody(directory, Context.Configuration.NamingConventionSwitcher.ConvertCodedToDefault);
        }

        public async Task<Options> OptionsAsync()
        {
            RestDirectoryInfo info = await Directory.GetInfoAsync();

            return new Options
            {
                Description = "A directory containing collections",
                AllowedMethods = new[]
                {
                    new OptionsMethod("GET", "Lists the available collections in this directory")
                },
                SubResources = info.Resources
            };
        }

        public Task<Feedback> CommandAsync(UnsafeMethod method, ResourceBody body)
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