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
            return Context.Services.EndpointResolver.GetFromResource(Context, resource);
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

        public Task<Feedback> CommandAsync(UnsafeMethod method, ResourceBody body)
        {
            ICommandStrategy<IRestItem> strategy = Context.Services.Strategies.ForItems.GetOrThrow(method);
            return strategy.ExecuteAsync(Item, Context, body);
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return true;
            // TODO: implement precondition checks
        }
    }
}