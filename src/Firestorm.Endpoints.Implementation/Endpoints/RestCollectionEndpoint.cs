using System;
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
            string dictionaryPrefix = Context.QueryString.DictionaryReferencePrefix;
            if (path.Raw.StartsWith(dictionaryPrefix))
            {
                string identifierName = path.GetCoded(dictionaryPrefix.Length);
                IRestDictionary dictionary = Collection.ToDictionary(identifierName);
                return new RestDictionaryEndpoint(Context, dictionary);
            }
            else
            {
                // TODO split by = char? see https://stackoverflow.com/a/20386425/369247
                IRestItem item = Collection.GetItem(path.Raw);
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