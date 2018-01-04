using System;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace Firestorm.Tests.Unit.Client
{
    public class DataContractImporterTests
    {
        [Fact]
        public void GetValues_TestModel_CorrectValues()
        {
            var importer = new DataContractImporter();

            var values = importer.GetValues(new TestModel
            {
                Id = 123,
                Name = "Fred",
                Date = new DateTime(2000, 01, 01)
            });

            var dict = values.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            Assert.Equal(123, dict["id"]);
            Assert.Equal("Fred", dict["name"]);
            Assert.Equal(new DateTime(2000, 01, 01), dict["date"]);
        }

        [DataContract]
        private class TestModel
        {
            [DataMember(Name = "id")]
            public int Id { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "date")]
            public DateTime Date { get; set; }
        }
    }
}
