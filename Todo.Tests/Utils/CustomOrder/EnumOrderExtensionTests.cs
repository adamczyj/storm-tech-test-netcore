using System;
using Todo.Utils.CustomOrder;
using Xunit;

namespace Todo.Tests.Utils.CustomOrder
{
    public class EnumOrderExtensionTests
    {
        [Fact]
        public void GetOrderValue_EnumHasOrderAttribute_ReturnsOrderValue()
        {
            //Arrange
            var testValue = TestEnum.TestValue;

            //Act
            var order = testValue.GetOrderValue();

            //Assert
            Assert.Equal(V1Order, order);
        }

        [Fact]
        public void GetOrderValue_EnumWithoutOrderAttribute_ThrowsEnumValueHasNoOrderAttributeException()
        {
            //Arrange
            var testValue = TestEnumWithoutCustomOrder.TestValue;

            //Act
            Action act = () => testValue.GetOrderValue();

            //Assert
            Assert.Throws<EnumValueHasNoOrderAttributeException>(act);
        }

        private enum TestEnum
        {
            [Order(V1Order)]
            TestValue
        }

        private const int V1Order = 2;

        private enum TestEnumWithoutCustomOrder
        {
            TestValue
        }
    }
}
