using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Models.TodoItems;
using Todo.Services;

namespace Todo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        [HttpGet("{id}/items")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TodoItemSummaryViewmodel>))]
        public async Task<IEnumerable<TodoItemSummaryViewmodel>> Get(int id, [FromQuery]TodoItemsQuery query) => await _todoListService.GetItemsAsync(id, query);
    }
}
