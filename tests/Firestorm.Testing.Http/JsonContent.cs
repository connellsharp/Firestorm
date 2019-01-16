using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Firestorm.Testing.Http
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj)
            : this(JsonConvert.SerializeObject(obj))
        {
        }

        public JsonContent(string json)
            : base(json, Encoding.UTF8, "application/json")
        {
        }
    }
}