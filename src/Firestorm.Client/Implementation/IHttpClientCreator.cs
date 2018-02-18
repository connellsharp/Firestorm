using System.Net.Http;

namespace Firestorm.Client
{
    internal interface IHttpClientCreator
    {
        HttpClient Create();
    }
}