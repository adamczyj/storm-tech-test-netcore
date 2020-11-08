using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Models.TodoItems;
using Todo.Utils.CustomOrder;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        public bool HideDone { get; set; }
        public ItemsOrderOption OrderBy { get; set; }

        public ICollection<TodoItemSummaryViewmodel> GetOrderedItems()
        {
            if (OrderBy == ItemsOrderOption.ByImportance)
                return Items.OrderBy(x => x.Importance.GetOrderValue()).ToList();

            if (OrderBy == ItemsOrderOption.ByRank)
                return Items.OrderBy(x => x.Rank).ToList();

            throw new ArgumentOutOfRangeException($"Order option: {OrderBy} is not handled.");
        }

        public TodoListDetailViewmodel(
            int todoListId,
            string title,
            ICollection<TodoItemSummaryViewmodel> items)
        {
            Items = items;
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