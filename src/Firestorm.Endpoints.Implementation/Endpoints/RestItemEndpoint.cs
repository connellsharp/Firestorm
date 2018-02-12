using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Strategies;

namespace Firestorm.Endpoints
{
    internal class RestItemEndpoint : IRestEndpoint
    {
        internal RestItemEndpoint(IRestEndpointContext endpointContext, IRestItem item)
        {
            Context = endpointContext;
            Item = item;
        }

        IRestEndpointContext Context { get; }

        IRestItem Item { get; }

        public IRestEndpoint Next(string fieldName)
        {
            IRestResource resource = Item.GetField(fieldName);
            return Endpoint.GetFromResource(Context, resource);
        }

        public async Task<ResourceBody> GetAsync()
        {
            IRestCollectionQuery query = Context.GetQuery();
            RestItemData item = await Item.GetDataAsync(query?.SelectFields);
            return new ItemBody(item);
        }

        public Task<Options> OptionsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Feedback> UnsafeAsync(UnsafeMethod method, ResourceBody body)
        {
            IUnsafeRequestStrategy<IRestItem> strategy = Context.Configuration.RequestStrategies.ForItems.GetOrThrow(method);
            return strategy.ExecuteAsync(Item, Context, body);
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return true;
            // TODO: implement precondition checks
        }
    }
}