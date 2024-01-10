using Microsoft.EntityFrameworkCore;
using ToDoListApp.DatabaseContext;
using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ToDoDbContext _dbContext;

        public ToDoService (ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
            if (_dbContext.ToDoItems.Count() == 0)
            {
                _dbContext.ToDoItems.Add(new ToDoItemModel { Name = "Item1" });
                _dbContext.SaveChanges();
            }
        }

        public async Task<List<ToDoItemModel>> GetAllItems()
        {
            var items = await _dbContext.ToDoItems.ToListAsync();
            return items;
        }

        public async Task<ToDoItemModel> GetById(int id)
        {
            var item = await _dbContext.ToDoItems.FindAsync(id);
            return item;
        }

        public async void AddItem(ToDoItemModel item)
        {
            _dbContext.ToDoItems.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ToDoItemModel> UpdateItem(int id, UpdateItemRequestModel request)
        {
            var toDoItem = await _dbContext.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return toDoItem;
            }

            toDoItem.Name = request.Name;
            toDoItem.IsComplete = request.IsComplete;

            _dbContext.ToDoItems.Update(toDoItem);
            await _dbContext.SaveChangesAsync();

            return toDoItem;
        }

        public async void DeleteItem(ToDoItemModel request)
        {
            _dbContext.ToDoItems.Remove(request);
            await _dbContext.SaveChangesAsync();
        }
    }
}
