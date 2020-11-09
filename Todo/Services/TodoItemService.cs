using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;
using Todo.Services.Gravatar;

namespace Todo.Services
{
    public interface ITodoItemService
    {
        Task SetRankAsync(int id, UpdateTodoItemRankModel rank);
        Task<int> CreateAsync(TodoItemCreateFields fields);
        Task<IEnumerable<TodoItemSummaryViewmodel>> GetItemsAsync(int todoListId, TodoItemsQuery query);
    }

    //Definitely I would extract data access layer to some Repository.It would make code unit testing easier.
    //This code is pretty simple but I could add some unit tests. We could test sorting and filtering.
    //I'm open to talk about testing approach it is very interesting topic :)
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IGravatarClient _gravatarClient;

        public TodoItemService(ApplicationDbContext dbContext, IGravatarClient gravatarClient)
        {
            _dbContext = dbContext;
            _gravatarClient = gravatarClient;
        }

        public async Task<int> CreateAsync(TodoItemCreateFields fields)
        {
            var item = new TodoItem(fields.TodoListId, fields.ResponsiblePartyId, fields.Title, fields.Importance, fields.Rank);

            await _dbContext.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return item.TodoItemId;
        }

        public async Task SetRankAsync(int id, UpdateTodoItemRankModel rank)
        {
            var todoItem = _dbContext.TodoItems.Single(x => x.TodoItemId == id);

            todoItem.Rank = rank.Rank;

            await _dbContext.SaveChangesAsync();
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

            return items;

            IQueryable<TodoItem> SortItems()
            {
                if (query.OrderBy == ItemsOrderOption.ByImportance)
                {
                    itemsQuery = itemsQuery.OrderBy(x => x.Importance);
                }

                if (query.OrderBy == ItemsOrderOption.ByRank)
                {
                    itemsQuery = itemsQuery.OrderByDescending(x => x.Rank);
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
