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

        public void AddItem(AddUpdateItemRequestModel request)
        {
            var item = new ToDoItemModel
            {
                Id = GenerateId(),
                Name = request.Name,
                IsComplete = request.IsComplete,
            };
            _dbContext.ToDoItems.Add(item);
            _dbContext.SaveChanges();
        }

        public ToDoItemModel UpdateItem(int id, AddUpdateItemRequestModel request)
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

        private int GenerateId()
        {
            int generatedId;
            var latestItem = _dbContext.ToDoItems.OrderByDescending(x => x.Id).FirstOrDefault();

            if (latestItem == null || string.IsNullOrEmpty(latestItem.Id.ToString()))
            {
                generatedId = 1;
                return generatedId;
            }

            var latestId = latestItem.Id;
            generatedId = latestItem.Id + 1;

            return generatedId;
        }
    }
}
