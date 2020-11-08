using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;
using Todo.Services.Gravatar;

namespace Todo.Services
{
    public interface ITodoListService
    {
        Task<TodoListDetailViewmodel> GetTodoListDetailAsync(int todoListId, bool showDoneOnly);
        Task<IEnumerable<TodoItemSummaryViewmodel>> GetItemsAsync(int todoListId, TodoItemsQuery query);
    }

    public class TodoListService : ITodoListService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IGravatarClient _gravatarClient;

        public TodoListService(ApplicationDbContext dbContext, IGravatarClient gravatarClient)
        {
            _dbContext = dbContext;
            _gravatarClient = gravatarClient;
        }

        public async Task<TodoListDetailViewmodel> GetTodoListDetailAsync(int todoListId, bool hideDone)
        {
            var todoList = _dbContext.TodoLists
                .Include(tl => tl.Owner)
                .Single(tl => tl.TodoListId == todoListId);

            todoList.Items = _dbContext.TodoItems
                .Include(x => x.ResponsibleParty)
                .Where(x => x.TodoListId == todoListId && (!hideDone || !x.IsDone))
                .ToList();

            var vm = TodoListDetailViewmodelFactory.Create(todoList);
            vm.HideDone = hideDone;

            await FillGravatarInfoAsync(vm.Items);

            return vm;
        }

        public async Task<IEnumerable<TodoItemSummaryViewmodel>> GetItemsAsync(int todoListId, TodoItemsQuery query)
        {
            var itemsQuery = _dbContext
                .TodoItems
                .Include(x => x.ResponsibleParty)
                .Where(x => x.TodoListId == todoListId && (!query.HideDone || !x.IsDone));

            itemsQuery = SortItems();

            var items = itemsQuery
                .ToList()
                .Select(TodoItemSummaryViewmodelFactory.Create)
                .ToList();

            await FillGravatarInfoAsync(items);

            return items.ToList();

            IQueryable<TodoItem> SortItems()
            {
                if (query.OrderBy == ItemsOrderOption.ByImportance)
                {
                    itemsQuery = itemsQuery.OrderBy(x => x.Importance);
                }

                if (query.OrderBy == ItemsOrderOption.ByRank)
                {
                    itemsQuery = itemsQuery.OrderBy(x => x.Rank);
                }

                return itemsQuery;
            }
        }

        private async Task FillGravatarInfoAsync(IEnumerable<TodoItemSummaryViewmodel> items)
        {
            foreach (var item in items)
            {
                var user = item.ResponsibleParty;
                user.UserName = await _gravatarClient.GetUserNameAsync(user.Email);
                item.Hash = _gravatarClient.GetHash(user.Email);
            }
        }
    }
}
