using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Client.Query;
using JetBrains.Annotations;

namespace Firestorm.Client
{
    public class RestClientCollection : RestClientResourceBase, IRestCollection
    {
        internal RestClientCollection(IHttpClientCreator httpClientCreator, string path)
            : base(httpClientCreator, path)
        { }

        public async Task<RestCollectionData> QueryDataAsync(IRestCollectionQuery query)
        {
            using (HttpClient client = HttpClientCreator.Create())
            {
                var queryStringBuilder = new CollectionQueryStringBuilder(new CollectionQueryStringConfiguration());
                string fullUrl = UriUtilities.AppendQueryString(Path, queryStringBuilder.BuildString(query));

                HttpResponseMessage response = await client.GetAsync(fullUrl);
                var queriedData = await Serializer.DeserializeAsync<IEnumerable<RestItemData>>(response);

                return new RestCollectionData(queriedData, null); // TODO: fill page details
            }
        }

        public IRestItem GetItem(string identifier, string identifierName = null)
        {
            if (identifierName != null)
                identifier = identifierName + "=" + identifier; // TODO: or even a / char?

            return new RestClientItem(HttpClientCreator, UriUtilities.AppendPath(Path, identifier));
        }

        public IRestDictionary ToDictionary(string identifierName)
        {
            throw new NotImplementedException("Dictionaries not implemented in client yet.");
        }

        public async Task<CreatedItemAcknowledgment> AddAsync(RestItemData itemData)
        {
            using (HttpClient client = HttpClientCreator.Create())
            {
                HttpResponseMessage response = await client.PostAsync(Path, Serializer.SerializeItemToContent(itemData));
                Acknowledgment acknowledgment = await EnsureSuccessAsync(response);
                var created = acknowledgment as CreatedItemAcknowledgment;

                if (created == null)
                    throw new InvalidOperationException("REST API did not return status code 201 after creating an item.");

                return created;
            }
        }
    }
}