using System;
using Firestorm.Core.Web;
using Newtonsoft.Json;

namespace Firestorm.Endpoints.Formatting.Json
{
    public class ResourceBodyJsonConverter : JsonGenericReadOnlyConverterBase<ResourceBody>
    {
        private readonly ResourceBodyReader _resourceBodyReader;

        public ResourceBodyJsonConverter(INamingConventionSwitcher switcher)
        {
            _resourceBodyReader = new ResourceBodyReader(switcher);
        }

        protected override ResourceBody ReadFromJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return _resourceBodyReader.ReadResourceBody(reader);
        }
    }
}