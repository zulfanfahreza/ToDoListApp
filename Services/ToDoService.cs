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

        public async Task<ToDoItemModel> AddItem(AddUpdateItemRequestModel request)
        {
            var item = new ToDoItemModel
            {
                Id = GenerateId(),
                Name = request.Name,
                IsComplete = request.IsComplete,
                CreatedAt = DateTime.Now
            };
            _dbContext.ToDoItems.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<ToDoItemModel> UpdateItem(int id, AddUpdateItemRequestModel request)
        {
            var toDoItem = _dbContext.ToDoItems.Where(x => x.Id.Equals(id)).SingleOrDefault();
            if (toDoItem == null)
            {
                return toDoItem;
            }

            toDoItem.Name = request.Name;
            toDoItem.IsComplete = request.IsComplete;
            toDoItem.UpdatedAt = DateTime.Now;

            _dbContext.ToDoItems.Update(toDoItem);
            await _dbContext.SaveChangesAsync();

            return toDoItem;
        }

        public async Task DeleteItem(ToDoItemModel request)
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
