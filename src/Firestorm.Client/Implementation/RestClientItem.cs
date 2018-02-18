using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Firestorm.Client
{
    public class RestClientItem : RestClientResourceBase, IRestItem
    {
        internal RestClientItem(IHttpClientCreator httpClientCreator, string path)
            : base(httpClientCreator, path)
        {
        }

        public IRestResource GetField(string fieldName)
        {
            // TODO sub collections and items?
            // Helper in ScalarFieldHelper, but that's the engine..

            return new RestClientScalar(HttpClientCreator, UriUtilities.AppendPath(Path, fieldName));
        }

        public async Task<RestItemData> GetDataAsync(IEnumerable<string> fieldNames)
        {
            using (HttpClient client = HttpClientCreator.Create())
            {
                var fullUri = UriUtilities.AppendQueryString(Path, GetFieldNamesQueryString(fieldNames));
                HttpResponseMessage response = await client.GetAsync(fullUri);
                return await Serializer.DeserializeAsync<RestItemData>(response);
            }
        }

        private static string GetFieldNamesQueryString([CanBeNull] IEnumerable<string> fieldNames)
        {
            if (fieldNames == null)
                return null;

            return "fields=" + string.Join(",", fieldNames); // TODO: review key and join char
        } 

        public async Task<Acknowledgment> EditAsync(RestItemData itemData)
        {
            using (HttpClient client = HttpClientCreator.Create())
            {
                HttpResponseMessage response = await client.PutAsync(Path, Serializer.SerializeItemToContent(itemData));
                return await EnsureSuccessAsync(response);
            }
        }

        public async Task<Acknowledgment> DeleteAsync()
        {
            using (HttpClient client = HttpClientCreator.Create())
            {
                HttpResponseMessage response = await client.DeleteAsync(Path);
                return await EnsureSuccessAsync(response);
            }
        }
    }
}