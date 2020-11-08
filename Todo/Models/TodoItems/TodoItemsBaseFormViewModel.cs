using System.ComponentModel.DataAnnotations;
using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public abstract class TodoItemsFormViewModel
    {
        public int TodoListId { get; set; }
        public string Title { get; set; }
        public string TodoListTitle { get; set; }
        public Importance Importance { get; set; }
        public int Rank { get; set; }

        [Display(Name = "Responsible person")]
        public string ResponsiblePartyId { get; set; }
    }
}
