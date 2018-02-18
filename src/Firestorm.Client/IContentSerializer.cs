using System.Net.Http;
using System.Threading.Tasks;

namespace Firestorm.Client
{
    internal interface IContentSerializer
    {
        Task<T> DeserializeAsync<T>(HttpResponseMessage response);

        StringContent SerializeItemToContent(object obj);
    }
}