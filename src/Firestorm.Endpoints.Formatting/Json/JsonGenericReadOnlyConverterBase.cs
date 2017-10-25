using System;
using Newtonsoft.Json;

namespace Firestorm.Endpoints.Formatting.Json
{
    public abstract class JsonGenericReadOnlyConverterBase<T> : JsonConverter
    {
        /// <remarks>
        /// This is very important, otherwise serialization breaks!
        /// </remarks>
        public sealed override bool CanWrite
        {
            get { return false; }
        }

        public sealed override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public sealed override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadFromJson(reader, objectType, existingValue, serializer);
        }

        protected abstract T ReadFromJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);
    }
}