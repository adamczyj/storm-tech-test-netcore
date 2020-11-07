using System;
using Todo.Utils;
using Xunit;

namespace Todo.Tests.Utils
{
    public class EnumAttributeGetterTests
    {
        [Fact]
        public void GetAttribute_EnumValueHasAttribute_ReturnsAttribute()
        {
            //Arrange
            var testValue = TestEnum.ValueWithAttribute;

            //Act
            var attribute = testValue.GetAttribute<CustomTestAttribute>();

            //Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public void GetAttribute_EnumValueHasntGotAttribute_ReturnsNull()
        {
            //Arrange
            var testValue = TestEnum.ValueWithoutAttribute;

            //Act
            var attribute = testValue.GetAttribute<CustomTestAttribute>();

            //Assert
            Assert.Null(attribute);
        }

        private class CustomTestAttribute : Attribute {}

        private enum TestEnum
        {
            [CustomTest]
            ValueWithAttribute,

            ValueWithoutAttribute
        }
    }
}
