using Todo.Models.TodoLists;

namespace Todo.Models.TodoItems
{
    public class TodoItemsQuery
    {
        public bool HideDone { get; set; }
        public ItemsOrderOption OrderBy { get; set; }
    }
}
