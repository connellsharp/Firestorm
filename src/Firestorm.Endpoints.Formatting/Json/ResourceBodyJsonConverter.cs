using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Firestorm.Core;
using Firestorm.Core.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Firestorm.Endpoints.Formatting.Json
{
    public class ResourceBodyJsonConverter : JsonGenericReadOnlyConverterBase<ResourceBody>
    {
        private static readonly JObjectConverter<RestItemData> Converter = new JObjectConverter<RestItemData>();

        protected override ResourceBody ReadFromJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadResourceBody(reader);
        }

        public static ResourceBody ReadResourceStream(Stream stream)
        {
            if (stream.CanSeek && stream.Length == 0)
                return null;

            using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
            {
                return ReadResourceBody(reader);
            }
        }

        public static ResourceBody ReadResourceBody(JsonReader reader)
        {
            JToken jBody = JToken.Load(reader);
            return GetResourceBody(jBody);
        }

        public static ResourceBody GetResourceBody(JToken jBody)
        {
            switch (jBody.Type)
            {
                case JTokenType.Array:
                    var items = from JObject jObject in jBody
                        select Converter.ConvertObject(jObject);

                    var collectionData = new RestCollectionData(items);
                    return new CollectionBody(collectionData);

                case JTokenType.Object:
                    Debug.Assert(jBody is JObject, "jBody is JObject");
                    RestItemData itemData = Converter.ConvertObject((JObject) jBody);
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