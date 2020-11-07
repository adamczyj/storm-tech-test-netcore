using System;
using System.Linq;

namespace Todo.Utils
{
    public static class EnumAttributeGetter
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();

            return enumType
                .GetField(Enum.GetName(enumType, value))
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}
