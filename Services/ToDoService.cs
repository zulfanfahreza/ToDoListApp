using Microsoft.EntityFrameworkCore;
using ToDoListApp.DatabaseContext;
using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoDbContext _dbContext;

        public ToDoService (IToDoDbContext dbContext)
        {
            _dbContext = dbContext;
            if (_dbContext.ToDoItems.Count() == 0)
            {
                _dbContext.ToDoItems.Add(new ToDoItemModel { Name = "Item1" });
                _dbContext.SaveChanges();
            }
        }

        public List<ToDoItemModel> GetAllItems()
        {
            var items = _dbContext.ToDoItems.ToList();
            return items;
        }

        public ToDoItemModel GetById(int id)
        {
            var item = _dbContext.ToDoItems.Find(id);
            return item;
        }

        public void AddItem(ToDoItemModel item)
        {
            _dbContext.ToDoItems.Add(item);
            _dbContext.SaveChanges();
        }

        public ToDoItemModel UpdateItem(int id, UpdateItemRequestModel request)
        {
            var toDoItem = _dbContext.ToDoItems.Find(id);
            if (toDoItem == null)
            {
                return toDoItem;
            }

            toDoItem.Name = request.Name;
            toDoItem.IsComplete = request.IsComplete;

            _dbContext.ToDoItems.Update(toDoItem);
            _dbContext.SaveChanges();

            return toDoItem;
        }

        public async void DeleteItem(ToDoItemModel request)
        {
            _dbContext.ToDoItems.Remove(request);
            await _dbContext.SaveChangesAsync();
        }
    }
}
