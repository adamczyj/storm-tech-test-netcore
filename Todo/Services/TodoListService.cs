using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;

namespace Todo.Services
{
    public interface ITodoListService
    {
        TodoListDetailViewmodel GetTodoListDetail(int todoListId, bool showDoneOnly);
    }

    public class TodoListService : ITodoListService
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoListService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TodoListDetailViewmodel GetTodoListDetail(int todoListId, bool hideDone)
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

            return vm;
        }
    }
}
