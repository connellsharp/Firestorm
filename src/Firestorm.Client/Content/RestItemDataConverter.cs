using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Firestorm.Client.Content
{
    /// <remarks>
    /// This class is kind-of temporary until the Client is full.
    /// TODO Should share code with or encapsulate with Endpoints.Formatting.JObjectToDictionaryTranslator.
    /// </remarks>
    internal class RestItemDataConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jToken = JToken.Load(reader);
            var jObject = jToken as JObject;
            return ConvertObject(jObject);
        }

        private object ConvertObject(JObject jObject)
        {
            var obj = new RestItemData();
            IDictionary<string, object> dictionary = obj;

            foreach (JProperty property in jObject.Properties())
            {
                string key = ConvertRequestedToCoded(property.Name);
                dictionary.Add(key, property.Value);
            }

            return obj;
        }

        private string ConvertRequestedToCoded(string propertyName)
        {
            return propertyName.MakeCamelCase('_', true);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RestItemData);
        }
    }
}