using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemCreateFields : TodoItemsFormViewModel
    {
        public TodoItemCreateFields()
        {
            Importance = Importance.Medium;
        }

        public TodoItemCreateFields(int todoListId)
        {
            TodoListId = todoListId;
        }

        public TodoItemCreateFields(int todoListId, string todoListTitle, string responsiblePartyId) : this()
        {
            TodoListId = todoListId;
            TodoListTitle = todoListTitle;
            ResponsiblePartyId = responsiblePartyId;
        }
    }
}