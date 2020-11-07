using System;

namespace Todo.Utils.CustomOrder
{
    /// <summary>
    /// I decided to introduce custom OrderAttribute which can be used to differ display sort order from enum values.
    /// </summary>
    public class OrderAttribute : Attribute
    {
        public int Order { get; }

        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}
