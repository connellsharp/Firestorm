using System.IO;
using Firestorm.Endpoints.Formatting.Json;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Json
{
    public class ResourceBodyJsonConverterTests
    {
        [Fact]
        public void ReadResourceBody_GivenObject_DoesntThrow()
        {
            var jsonReader = new JsonTextReader(new StringReader(@"{ id: 123, name: ""Fred"" }"));
            var body = ResourceBodyJsonConverter.ReadResourceBody(jsonReader);
            Assert.Equal(ResourceType.Item, body.ResourceType);
        }
        [Fact]
        public void ReadResourceBody_GivenArray_DoesntThrow()
        {
            var jsonReader = new JsonTextReader(new StringReader(@"[ { id: 123, name: ""Fred"" }, { id: 321, name: ""Derf"" } ]"));
            var body = ResourceBodyJsonConverter.ReadResourceBody(jsonReader);
            Assert.Equal(ResourceType.Collection, body.ResourceType);
        }
    }
}