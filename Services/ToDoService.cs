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

        public List<ToDoItemModel> GetAllItems()
        {
            var items = _dbContext.ToDoItems.ToList();
            return items;
        }

        public ToDoItemModel GetById(int id)
        {
            var item = _dbContext.ToDoItems.Where(x => x.Id.Equals(id)).SingleOrDefault();
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
            var toDoItem = _dbContext.ToDoItems.Where(x => x.Id.Equals(id)).SingleOrDefault();
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

        public void DeleteItem(ToDoItemModel request)
        {
            _dbContext.ToDoItems.Remove(request);
            _dbContext.SaveChanges();
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
