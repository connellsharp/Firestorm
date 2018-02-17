using System;
using System.Dynamic;
using Firestorm.Endpoints.Formatting.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Json
{
    public class JObjectConverterTests
    {
        public JObjectConverterTests()
        {
            DictionaryTranslator = new JObjectToDictionaryTranslator<ExpandoObject>(null);
        }

        private JObjectToDictionaryTranslator<ExpandoObject> DictionaryTranslator { get; set; }

        [Fact]
        public void Null_Convert_IsNull()
        {
            var jValue = JValue.CreateNull();
            dynamic dyn = DictionaryTranslator.Convert(jValue);
            Assert.Null(dyn);
        }

        [Fact]
        public void SimpleObject_Convert_IsCopy()
        {
            var obj = new
            {
                thing = "yeah",
                test = "safe",
                number = 123
            };

            var jObject = JObject.FromObject(obj);
            dynamic dyn = DictionaryTranslator.ConvertObject(jObject);

            Assert.Equal(obj.thing, dyn.thing);
            Assert.Equal(obj.test, dyn.test);
            Assert.Equal(obj.number, dyn.number);
        }

        [Fact]
        public void SimpleObjectWithNulls_Convert_IsCopy()
        {
            var obj = new
            {
                thing = (string)null,
                again = (int?)null,
            };

            var jObject = JObject.FromObject(obj);
            dynamic dyn = DictionaryTranslator.ConvertObject(jObject);

            Assert.Equal(obj.thing, dyn.thing);
            Assert.Equal(obj.again, dyn.again);
        }

        [Fact]
        public void ComplexNestedObject_Convert_IsCopy()
        {
            var obj = new
            {
                thing = "yeah",
                test = new
                {
                    nested = "holycrap",
                    what = new { the = "fuuuuuuuuuu", number = 123 }
                },
                numberAgain = 321321
            };

            var jObject = JObject.FromObject(obj);
            dynamic dyn = DictionaryTranslator.ConvertObject(jObject);

            Assert.Equal(obj.thing, dyn.thing);
            Assert.Equal(obj.test.nested, dyn.test.nested);
            Assert.Equal(obj.test.what.the, dyn.test.what.the);
            Assert.Equal(obj.numberAgain, dyn.numberAgain);
        }

        [Fact]
        public void NestedArray_Convert_IsCopy()
        {
            var arr = new[] {
                new { test = "yeah" },
                new { test = "safe" },
                new { test = "thing" },
            };

            var jToken = JToken.FromObject(arr);
            dynamic dyn = DictionaryTranslator.Convert(jToken);

            Assert.Equal(arr[0].test, arr[0].test);
            Assert.Equal(arr[1].test, arr[1].test);
            Assert.Equal(arr[2].test, arr[2].test);
        }

        [Fact]
        public void Comment_Convert_ThrowsNotSupported()
        {
            var jValue = JValue.CreateComment("This is a comment");
            Assert.Throws<NotSupportedException>(delegate
            {
                DictionaryTranslator.Convert(jValue);
            });
        }
    }
}