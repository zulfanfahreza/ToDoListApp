using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ToDoListApp.DatabaseContext;
using ToDoListApp.Models;
using ToDoListApp.Utilities;

namespace ToDoListApp.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoDbContext _dbContext;
        private readonly ILogging _logger;

        public ToDoService (IToDoDbContext dbContext, ILogging logging)
        {
            _dbContext = dbContext;
            _logger = logging;
        }

        public async Task<List<ToDoItemModel>> GetAllItems()
        {
            _logger.LogDebug("ToDoService.GetAllItems", "Start getting all items from db");
            var items = await _dbContext.ToDoItems.ToListAsync();
            return items;
        }

        public async Task<ToDoItemModel> GetById(int id)
        {
            _logger.LogDebug("ToDoService.GetById", $"Start getting item from db with Id: {id}");
            var item = await _dbContext.ToDoItems.FindAsync(id);
            _logger.LogDebug($"{nameof(ToDoService.GetById)}", $"Found item: {JsonConvert.SerializeObject(item)}");
            return item;
        }

        public async Task<ToDoItemModel> AddItem(AddUpdateItemRequestModel request)
        {
            _logger.LogDebug("ToDoService.AddItem", $"Start adding item to db with request: {JsonConvert.SerializeObject(request)}");
            var item = new ToDoItemModel
            {
                Name = request.Name,
                IsComplete = request.IsComplete,
                CreatedAt = DateTime.Now
            };
            _logger.LogDebug("ToDoService.AddItem", $"Added Item: {JsonConvert.SerializeObject(item)}");
            _dbContext.ToDoItems.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<ToDoItemModel> UpdateItem(int id, AddUpdateItemRequestModel request)
        {
            _logger.LogDebug("ToDoService.UpdateItem", $"Start updating item from db with Id: {id} and request: {JsonConvert.SerializeObject(request)}");
            var toDoItem = _dbContext.ToDoItems.Where(x => x.Id.Equals(id)).SingleOrDefault();
            if (toDoItem == null)
            {
                return toDoItem;
            }

            toDoItem.Name = request.Name;
            toDoItem.IsComplete = request.IsComplete;
            toDoItem.UpdatedAt = DateTime.Now;
            _logger.LogDebug("ToDoService.UpdateItem", $"Updated Item: {JsonConvert.SerializeObject(toDoItem)}");
            _dbContext.ToDoItems.Update(toDoItem);
            await _dbContext.SaveChangesAsync();

            return toDoItem;
        }

        public async Task DeleteItem(ToDoItemModel request)
        {
            _logger.LogDebug("ToDoService.DeleteItem", $"Start deleting item from db with request: {JsonConvert.SerializeObject(request)}");
            _dbContext.ToDoItems.Remove(request);
            await _dbContext.SaveChangesAsync();
        }

        private int GenerateId()
        {
            _logger.LogDebug("ToDoService.GenerateId", $"Start generating Id");
            int generatedId;
            var latestItem = _dbContext.ToDoItems.OrderByDescending(x => x.Id).FirstOrDefault();
            _logger.LogDebug("ToDoService.GenerateId", $"Latest item: {JsonConvert.SerializeObject(latestItem)}");

            if (latestItem == null || string.IsNullOrEmpty(latestItem.Id.ToString()))
            {
                generatedId = 1;
                _logger.LogDebug("ToDoService.GenerateId", $"Generated Id: {JsonConvert.SerializeObject(generatedId)}");
                return generatedId;
            }

            var latestId = latestItem.Id;
            generatedId = latestItem.Id + 1;
            _logger.LogDebug("ToDoService.GenerateId", $"Latest item Id: {JsonConvert.SerializeObject(latestId)}");
            _logger.LogDebug("ToDoService.GenerateId", $"Generated Id: {JsonConvert.SerializeObject(generatedId)}");

            return generatedId;
        }
    }
}
