using System;

namespace Todo.Utils.CustomOrder
{
    public static class EnumOrderExtension
    {
        public static int GetOrderValue(this Enum value) => value.GetAttribute<OrderAttribute>()?.Order ?? throw new EnumValueHasNoOrderAttributeException(value);
    }
}