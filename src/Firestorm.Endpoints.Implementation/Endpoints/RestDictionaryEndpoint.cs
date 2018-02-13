using System;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Pagination;
using Firestorm.Endpoints.Preconditions;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// REST endpoint to handle <see cref="IRestDictionary"/> implementations.
    /// </summary>
    internal class RestDictionaryEndpoint : IRestEndpoint
    {
        internal RestDictionaryEndpoint(IRestEndpointContext endpointContext, IRestDictionary dictionary)
        {
            Context = endpointContext;
            Dictionary = dictionary;
        }

        public IRestEndpointContext Context { get; }

        public IRestDictionary Dictionary { get; }

        public IRestEndpoint Next(string identifier)
        {
            IRestItem item = Dictionary.GetItem(identifier);
            return new RestItemEndpoint(Context, item);
        }

        /// <remarks>Similar body to <see cref="RestCollectionEndpoint.GetAsync"/></remarks>
        public async Task<ResourceBody> GetAsync(IRestCollectionQuery query)
        {
            QueryValidationUtility.EnsureValidQuery(query);

            RestDictionaryData dictionaryData = await Dictionary.QueryDataAsync(query);
            
            var linkCalculator = new PageLinkCalculator(Context.Configuration.ResponseConfiguration.PageConfiguration, query?.PageInstruction, dictionaryData.PageDetails);
            PageLinks pageLinks = linkCalculator.Calculate();

            return new DictionaryBody(dictionaryData.Items, pageLinks);
        }

        public Task<Options> OptionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Feedback> UnsafeAsync(UnsafeMethod method, ResourceBody body)
        {
            throw new NotImplementedException("Not implemented unsafe strategies on dictionaries yet.");
            // TODO could implement PUTs with multi updates ?
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return true;
            // TODO: implement precondition checks
        }
    }
}