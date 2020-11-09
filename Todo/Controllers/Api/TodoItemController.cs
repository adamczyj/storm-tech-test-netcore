﻿using System.Net;
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
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _itemService;

        public TodoItemController(ITodoItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(int))]
        public async Task<int> Create([FromBody] TodoItemCreateFields fields) => await _itemService.CreateAsync(fields);


        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task UpdateRank(int id, [FromBody] UpdateTodoItemRankModel rankModel) => await _itemService.SetRankAsync(id, rankModel);
    }
}
