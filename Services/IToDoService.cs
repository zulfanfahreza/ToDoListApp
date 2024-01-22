using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public interface IToDoService
    {
        Task<List<ToDoItemModel>> GetAllItems();
        Task<ToDoItemModel> GetById(int id);
        Task<ToDoItemModel> AddItem(AddUpdateItemRequestModel request);
        Task<ToDoItemModel> UpdateItem(int id, AddUpdateItemRequestModel request);
        Task DeleteItem(ToDoItemModel request);
    }
}
