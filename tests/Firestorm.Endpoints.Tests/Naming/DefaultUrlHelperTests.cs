using Firestorm.Endpoints.Executors;
using Xunit;

namespace Firestorm.Endpoints.Tests.Naming
{
    public class DefaultUrlHelperTests
    {
        [Fact]
        public void NormalString_GetIdentifier_ValueIsSameString()
        {
            var calculator = new DefaultUrlHelper(new UrlConfiguration());
            string path = "pathstring";

            var info = calculator.GetIdentifierInfo(new RawNextPath(path));
            
            Assert.Equal(path, info.Value);
        }
        
        [Fact]
        public void DictionaryString_GetIdentifier_IsDictionary()
        {
            var calculator = new DefaultUrlHelper(new UrlConfiguration());
            string path = "by_id";

            var info = calculator.GetIdentifierInfo(new RawNextPath(path));
            
            Assert.Equal(true, info.IsDictionary);
        }
        
        [Fact]
        public void DictionaryString_GetIdentifier_NameIsCorrect()
        {
            var calculator = new DefaultUrlHelper(new UrlConfiguration());
            string path = "by_id";

            var info = calculator.GetIdentifierInfo(new RawNextPath(path));
            
            Assert.Equal("id", info.Name);
        }
        
        [Fact]
        public void EqualsString_GetIdentifier_NameAndValueSet()
        {
            var calculator = new DefaultUrlHelper(new UrlConfiguration());
            string path = "id=123";

            var info = calculator.GetIdentifierInfo(new RawNextPath(path));
            
            Assert.Equal("id", info.Name);
            Assert.Equal("123", info.Value);
        }
    }
}