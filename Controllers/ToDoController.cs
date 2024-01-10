﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;
using ToDoListApp.DatabaseContext;
using Microsoft.AspNetCore.Authorization;
using ToDoListApp.Services;

namespace ToDoListApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemModel>>> GetToDoItems()
        {
            var items = _toDoService.GetAllItems();
            return await items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemModel>> GetToDoItem(int id)
        {
            var toDoItems = await _toDoService.GetById(id);
            if (toDoItems == null)
            {
                return NotFound();
            }

            return toDoItems;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItemModel>> PostToDoItem(ToDoItemModel toDoItem)
        {
            _toDoService.AddItem(toDoItem);

            return CreatedAtAction(nameof(GetToDoItem), new {id = toDoItem.Id}, toDoItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDoItemModel>> PutToDoItem(int id,  ToDoItemModel request)
        {
            var toDoItem = await _toDoService.UpdateItem(id, request);
            if (toDoItem == null)
            {
                return NotFound();
            }
            
            return toDoItem;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var toDoItem = await _toDoService.GetById(id);
            if (toDoItem == null) 
            { 
                return NotFound();
            }

            _toDoService.DeleteItem(toDoItem);
            return NoContent();
        }
    }
}
