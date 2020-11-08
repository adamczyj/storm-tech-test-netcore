using System.Linq;
using System.Threading.Tasks;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Models.TodoItems;

namespace Todo.Services
{
    public interface ITodoItemService
    {
        Task SetRankAsync(int id, UpdateTodoItemRankModel rank);
        Task<int> CreateAsync(TodoItemCreateFields fields);
    }

    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoItemService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}
