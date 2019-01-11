using System.Threading.Tasks;
using Firestorm.Endpoints.Requests;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    internal class RestItemEndpoint : IRestEndpoint
    {
        internal RestItemEndpoint(IEndpointContext endpointContext, IRestItem item)
        {
            Context = endpointContext;
            Item = item;
        }

        IEndpointContext Context { get; }

        IRestItem Item { get; }

        public IRestEndpoint Next(INextPath fieldName)
        {
            IRestResource resource = Item.GetField(fieldName.GetCoded());
            return Context.Configuration.Resolver.GetFromResource(Context, resource);
        }

        public async Task<ResourceBody> GetAsync(IRestCollectionQuery query)
        {
            RestItemData item = await Item.GetDataAsync(query?.SelectFields);
            return new ItemBody(item);
        }

        public async Task<Options> OptionsAsync()
        {
            return new Options
            {
                Description = "An item",
                AllowedMethods = new[]
                {
                    new OptionsMethod("GET", "Gets the item"),
                    new OptionsMethod("PUT", "Replaces the item"),
                    new OptionsMethod("DELETE", "Deletes the item")
                }
            };
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