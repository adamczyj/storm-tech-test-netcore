using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Models.TodoItems;
using Todo.Services;

namespace Todo.Controllers.Api
{
    [Route("api/todolist")]
    [ApiController]
    [Authorize]
    public class TodoListApiController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodoListApiController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpGet("{id}/items")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TodoItemSummaryViewmodel>))]
        public async Task<IEnumerable<TodoItemSummaryViewmodel>> GetItems(int id, [FromQuery]TodoItemsQuery query) => await _todoItemService.GetItemsAsync(id, query);
    }
}
