using System;
using Firestorm.Stems.Roots;
using Xunit;

namespace Firestorm.Tests.Unit.Stems.Roots
{
    public class AttributedTypeValidatorTests
    {
        private class FakeAttribute : Attribute
        {
        }

        [Fake]
        private class DecoratedClass
        {
        }
        
        private class NonDecoratedClass
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AttributedType_IsValid_ReturnsCorrectValue(bool shouldHaveAttribute)
        {
            var validator = new AttributedTypeValidator(typeof(FakeAttribute), shouldHaveAttribute);

            bool isValid = validator.IsValidType(typeof(DecoratedClass));

            Assert.Equal(shouldHaveAttribute, isValid);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void NonAttributedType_IsValid_ReturnsCorrectValue(bool shouldHaveAttribute)
        {
            var validator = new AttributedTypeValidator(typeof(FakeAttribute), shouldHaveAttribute);

            bool isValid = validator.IsValidType(typeof(NonDecoratedClass));

            Assert.Equal(!shouldHaveAttribute, isValid);
        }
    }
}
