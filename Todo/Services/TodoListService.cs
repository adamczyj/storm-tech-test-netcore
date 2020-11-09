using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;

namespace Todo.Services
{
    public interface ITodoListService
    {
        Task<TodoListDetailViewmodel> GetTodoListDetailAsync(int todoListId, bool showDoneOnly);
    }

    public class TodoListService : ITodoListService
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoListService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TodoListDetailViewmodel> GetTodoListDetailAsync(int todoListId, bool hideDone)
        {
            var todoList = await _dbContext.TodoLists
                .Include(tl => tl.Owner)
                .SingleAsync(tl => tl.TodoListId == todoListId);

            return TodoListDetailViewmodelFactory.Create(todoList);
        }
    }
}
