using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;
using ToDoListApp.DatabaseContext;

namespace ToDoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext _dbContext;

        public ToDoController(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
            if (_dbContext.ToDoItems.Count() == 0)
            {
                _dbContext.ToDoItems.Add(new ToDoItemModel { Name = "Item1" });
                _dbContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemModel>>> GetToDoItems()
        {
            return await _dbContext.ToDoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemModel>> GetToDoItem(int id)
        {
            var toDoItems = await _dbContext.ToDoItems.FindAsync(id);
            if (toDoItems == null)
            {
                return NotFound();
            }

            return toDoItems;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItemModel>> PostToDoItem(ToDoItemModel toDoItem)
        {
            _dbContext.ToDoItems.Add(toDoItem);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoItem), new {id = toDoItem.Id}, toDoItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDoItemModel>> PutToDoItem(int id,  ToDoItemModel item)
        {
            var toDoItem = await _dbContext.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            toDoItem.Name = item.Name;
            toDoItem.IsComplete = item.IsComplete;

            _dbContext.ToDoItems.Update(toDoItem);
            await _dbContext.SaveChangesAsync();

            return toDoItem;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var toDoItem = await _dbContext.ToDoItems.FindAsync(id);
            if (toDoItem == null) 
            { 
                return NotFound();
            }

            _dbContext.ToDoItems.Remove(toDoItem);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
