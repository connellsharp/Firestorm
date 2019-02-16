using System.Threading.Tasks;
using Firestorm.Endpoints.Query;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;
using Firestorm.Endpoints.Requests;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// REST endpoint to handle <see cref="IRestCollection"/> implementations.
    /// </summary>
    internal class RestCollectionEndpoint : IRestEndpoint
    {
        internal RestCollectionEndpoint(IEndpointContext endpointContext, IRestCollection collection)
        {
            Context = endpointContext;
            Collection = collection;
        }

        public IEndpointContext Context { get; }

        public IRestCollection Collection { get; }

        public IRestEndpoint Next(INextPath path)
        {
            var identifierInfo = Context.Services.UrlHelper.GetIdentifierInfo(path);

            if (identifierInfo.IsDictionary)
            {
                IRestDictionary dictionary = Collection.ToDictionary(identifierInfo.Name);
                return new RestDictionaryEndpoint(Context, dictionary);
            }
            else
            {
                IRestItem item = Collection.GetItem(identifierInfo.Value, identifierInfo.Name);
                return new RestItemEndpoint(Context, item);
            }
        }

        public async Task<ResourceBody> GetAsync(IRestCollectionQuery query)
        {
            QueryValidationUtility.EnsureValidQuery(query);

            RestCollectionData collectionData = await Collection.QueryDataAsync(query);

            PageLinks pageLinks =  Context.Services.PageLinkCalculator.Calculate(query?.PageInstruction, collectionData.PageDetails);

            return new CollectionBody(collectionData.Items, pageLinks);
        }

        public async Task<Options> OptionsAsync()
        {
            return new Options
            {
                Description = "A collection of items",
                AllowedMethods = new[]
                {
                    new OptionsMethod("GET", "Lists the items in the collection"),
                    new OptionsMethod("POST", "Adds a new item to the collection")
                }
            };
        }

        public Task<Feedback> CommandAsync(UnsafeMethod method, ResourceBody body)
        {
            ICommandStrategy<IRestCollection> strategy = Context.Services.Strategies.ForCollections.GetOrThrow(method);
            return strategy.ExecuteAsync(Collection, Context, body);
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return true;
            // TODO: implement precondition checks
        }
    }
}