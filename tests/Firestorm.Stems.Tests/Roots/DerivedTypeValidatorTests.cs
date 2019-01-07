using Firestorm.Stems.Roots;
using Xunit;

namespace Firestorm.Stems.Tests.Roots
{
    public class DerivedTypeValidatorTests
    {
        private abstract class BaseClass
        {
        }
        
        private class DerivedClass : BaseClass
        {
        }
        
        private class NonDerivedClass
        {
        }

        [Fact]
        public void DerivedType_IsValid_ReturnsTrue()
        {
            var validator = new DerivedTypeValidator(typeof(BaseClass));

            bool isValid = validator.IsValidType(typeof(DerivedClass));

            Assert.True(isValid);
        }

        [Fact]
        public void NonDerivedType_IsValid_ReturnsFalse()
        {
            var validator = new DerivedTypeValidator(typeof(BaseClass));

            bool isValid = validator.IsValidType(typeof(NonDerivedClass));

            Assert.False(isValid);
        }
    }
}