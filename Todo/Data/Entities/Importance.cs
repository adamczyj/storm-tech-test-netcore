using Todo.Utils.CustomOrder;

namespace Todo.Data.Entities
{
    public enum Importance
    {
        [Order(1)]
        High = 0,

        [Order(2)]
        Medium = 1,

        [Order(3)]
        Low = 2
    }
}