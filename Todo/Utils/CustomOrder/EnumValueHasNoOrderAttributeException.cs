using System;

namespace Todo.Utils.CustomOrder
{
    public class EnumValueHasNoOrderAttributeException : Exception
    {
        public EnumValueHasNoOrderAttributeException(Enum value) : base($"{value.GetType()}.{value} hasn't got OrderAttribute.")
        {
        }
    }
}