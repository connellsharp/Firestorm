using Firestorm.Stems.Attributes.Basic.Attributes;
using Xunit;

namespace Firestorm.Stems.Tests.Attributes
{
    public class AuthorizeAttributeTests
    {
        [Fact]
        public void NotAuthenticated_IsAuthorized_False()
        {
            var attribute = new AuthorizeAttribute();
            bool authorised = attribute.IsAuthorized(new TestRestUser());
            Assert.False(authorised);
        }

        [Fact]
        public void AuthedAsWrongName_IsAuthorized_False()
        {
            var attribute = new AuthorizeAttribute { Users = "Fred" };
            bool authorised = attribute.IsAuthorized(new TestRestUser { Username = "Bill" });
            Assert.False(authorised);
        }

        [Fact]
        public void AuthedAsRightName_IsAuthorized_True()
        {
            var attribute = new AuthorizeAttribute { Users = "Bill" };
            bool authorised = attribute.IsAuthorized(new TestRestUser { Username = "Bill" });
            Assert.True(authorised);
        }

        [Fact]
        public void AuthedAsWrongRoles_IsAuthorized_False()
        {
            var attribute = new AuthorizeAttribute { Users = "Bob", Roles = "MyRole,OtherRole" };
            bool authorised = attribute.IsAuthorized(new TestRestUser { Username = "Bob" });
            Assert.False(authorised);
        }

        [Fact]
        public void AuthedAsRightRole_IsAuthorized_True()
        {
            var attribute = new AuthorizeAttribute { Users = "Bob", Roles = "TestRole" };
            bool authorised = attribute.IsAuthorized(new TestRestUser { Username = "Bob" });
            Assert.True(authorised);
        }

        [Fact]
        public void AuthedAsRightRoleWrongUser_IsAuthorized_False()
        {
            var attribute = new AuthorizeAttribute { Users = "Bob", Roles = "TestRole" };
            bool authorised = attribute.IsAuthorized(new TestRestUser { Username = "Ben" });
            Assert.False(authorised);
        }
    }
}