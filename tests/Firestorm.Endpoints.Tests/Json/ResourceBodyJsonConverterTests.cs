using System.IO;
using Firestorm.Endpoints.Formatting.Json;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Endpoints.Tests.Json
{
    public class ResourceBodyJsonConverterTests
    {
        [Fact]
        public void ReadResourceBody_GivenObject_DoesntThrow()
        {
            var jsonReader = new JsonTextReader(new StringReader(@"{ id: 123, name: ""Fred"" }"));
            var reader = new ResourceBodyReader(null);
            var body = reader.ReadResourceBody(jsonReader);
            Assert.Equal(ResourceType.Item, body.ResourceType);
        }
        [Fact]
        public void ReadResourceBody_GivenArray_DoesntThrow()
        {
            var jsonReader = new JsonTextReader(new StringReader(@"[ { id: 123, name: ""Fred"" }, { id: 321, name: ""Derf"" } ]"));
            var reader = new ResourceBodyReader(null);
            var body = reader.ReadResourceBody(jsonReader);
            Assert.Equal(ResourceType.Collection, body.ResourceType);
        }
    }
}