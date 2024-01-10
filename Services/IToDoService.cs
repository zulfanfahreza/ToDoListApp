using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public interface IToDoService
    {
        Task<List<ToDoItemModel>> GetAllItems();
        Task<ToDoItemModel> GetById(int id);
        void AddItem(ToDoItemModel item);
        Task<ToDoItemModel> UpdateItem(int id, UpdateItemRequestModel request);
        void DeleteItem(ToDoItemModel request);
    }
}
