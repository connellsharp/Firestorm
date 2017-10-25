using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Firestorm.Endpoints.Formatting.Json
{
    /// <remarks>
    /// Taken from http://stackoverflow.com/a/12641541/369247 and abstracted to <see cref="JsonGenericReadOnlyConverterBase{T}"/>.
    /// </remarks>
    public abstract class JsonCreationConverter<T> : JsonGenericReadOnlyConverterBase<T>
    {
        /// <summary> 
        /// Create an instance of objectType, based properties in the JSON object 
        /// </summary> 
        /// <param name="objectType">type of object expected</param> 
        /// <param name="jObject">contents of JSON object that will be 
        /// deserialized</param> 
        /// <returns></returns> 
        protected abstract T Create(Type objectType, JObject jObject);

        protected override T ReadFromJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return default(T);
            
            JObject jObject = JObject.Load(reader);
            T target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}