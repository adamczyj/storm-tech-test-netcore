namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public bool HideDone { get; set; }
        public ItemsOrderOption OrderBy { get; set; }

        public TodoListDetailViewmodel(
            int todoListId,
            string title)
        {
            TodoListId = todoListId;
            Title = title;
        }
    }

    public enum ItemsOrderOption
    {
        ByImportance = 0,
        ByRank = 1
    }
}