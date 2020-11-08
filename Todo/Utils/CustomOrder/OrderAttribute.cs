using System;

namespace Todo.Utils.CustomOrder
{
    /// <summary>
    /// I know that this is overengineering :) We can talk why i decided to introduce this way of sort by enums.
    /// I decided to introduce custom OrderAttribute which can be used to differ display sort order from enum values.
    /// This attribute can be used when we sort at UI side, when we want to use it in EF Core query it can be problematic.
    /// It requires dynamic building expression tree which can be used by Select then OrderBy.
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
