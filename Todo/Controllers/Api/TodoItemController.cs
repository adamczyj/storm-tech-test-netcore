using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Models.TodoItems;

namespace Todo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoItemController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoItemController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(int))]
        public async Task<int> Post([FromBody] TodoItemCreateFields fields)
        {
            //We should handle validation here
            var item = new TodoItem(fields.TodoListId, fields.ResponsiblePartyId, fields.Title, fields.Importance, fields.Rank);

            await _dbContext.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return item.TodoItemId;
        }


    }
}
