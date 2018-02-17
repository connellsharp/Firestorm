using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Firestorm.Endpoints.Formatting.Json
{
    internal class RestItemDataJsonConverter : JsonConverter
    {
        private readonly JObjectToDictionaryTranslator<RestItemData> _restItemDataTranslator;

        public RestItemDataJsonConverter(INamingConventionSwitcher switcher)
        {
            _restItemDataTranslator = new JObjectToDictionaryTranslator<RestItemData>(switcher);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jBody = JToken.Load(reader);
            return _restItemDataTranslator.Convert(jBody);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(RestItemData).IsAssignableFrom(objectType) || typeof(IEnumerable<RestItemData>).IsAssignableFrom(objectType);
        }
    }
}