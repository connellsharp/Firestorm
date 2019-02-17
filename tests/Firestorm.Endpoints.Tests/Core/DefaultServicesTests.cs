using System.Linq;
using Xunit;

namespace Firestorm.Endpoints.Tests.Core
{
    public class DefaultServicesTests
    {
        [Fact]
        public void AddSingleton_GetService_RetrievesSingleton()
        {
            var builder = new DefaultServicesBuilder();

            var person = new Person();
            builder.Add(person);

            var containerPerson = builder.Build().GetService<Person>();
            
            Assert.Equal(person, containerPerson);
        }
        
        [Fact]
        public void AddSingletons_GetServices_RetrievesManySingletons()
        {
            var builder = new DefaultServicesBuilder();

            var person1 = new Person();
            builder.Add(person1);
            
            var person2 = new Person();
            builder.Add(person2);

            var containerPersons = builder.Build().GetServices<Person>().ToList();
            
            Assert.Equal(2, containerPersons.Count);
            Assert.Contains(person1, containerPersons);
            Assert.Contains(person2, containerPersons);
        }
        
        private class Person { }
    }
}