using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Firestorm.Core.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Firestorm.Endpoints.Formatting.Json
{
    /// <summary>
    /// Reads a JSON stream to get the <see cref="ResourceBody"/>.
    /// </summary>
    internal class ResourceBodyReader
    {
        private readonly JObjectDictionaryCreator<RestItemData> _restItemDataCreator;

        public ResourceBodyReader(INamingConventionSwitcher switcher)
        {
            _restItemDataCreator = new JObjectDictionaryCreator<RestItemData>(switcher);
        }

        internal ResourceBody ReadResourceStream(Stream stream)
        {
            if (stream.CanSeek && stream.Length == 0)
                return null;

            using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
            {
                return ReadResourceBody(reader);
            }
        }

        public ResourceBody ReadResourceBody(JsonReader reader)
        {
            JToken jBody = JToken.Load(reader);
            return GetResourceBody(jBody);
        }

        public ResourceBody GetResourceBody(JToken jBody)
        {
            switch (jBody.Type)
            {
                case JTokenType.Array:
                    var items = from JObject jObject in jBody
                                select _restItemDataCreator.ConvertObject(jObject);

                    return new CollectionBody(items, null);

                case JTokenType.Object:
                    Debug.Assert(jBody is JObject, "jBody is JObject");
                    RestItemData itemData = _restItemDataCreator.ConvertObject((JObject)jBody);
                    return new ItemBody(itemData);

                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Integer:
                case JTokenType.Date:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                    var obj = jBody.ToObject<object>();
                    return new ScalarBody(obj);
            }

            throw new FormatException("Cannot convert body type.");
        }
    }
}