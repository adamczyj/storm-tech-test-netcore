using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly ITodoListService _todoListService;

        public TodoListController(
            ApplicationDbContext dbContext, 
            IUserStore<IdentityUser> userStore, 
            ITodoListService todoListService)
        {
            _dbContext = dbContext;
            _userStore = userStore;
            _todoListService = todoListService;
        }

        public IActionResult Index()
        {
            var userId = User.Id();
            var todoLists = _dbContext.RelevantTodoLists(userId);
            var viewmodel = TodoListIndexViewmodelFactory.Create(todoLists);
            return View(viewmodel);
        }

        public IActionResult Detail(int todoListId, bool? hideDone, ItemsOrderOption? orderBy)
        {
            var vm = _todoListService.GetTodoListDetail(todoListId, hideDone ?? false);
            vm.OrderBy = orderBy ?? ItemsOrderOption.ByImportance;

            return View(vm);
        } 

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoListFields());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoListFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var currentUser = await _userStore.FindByIdAsync(User.Id(), CancellationToken.None);

            var todoList = new TodoList(currentUser, fields.Title);

            await _dbContext.AddAsync(todoList);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Create", "TodoItem", new {todoList.TodoListId});
        }
    }
}