using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public interface IToDoService
    {
        List<ToDoItemModel> GetAllItems();
        ToDoItemModel GetById(int id);
        void AddItem(AddUpdateItemRequestModel request);
        ToDoItemModel UpdateItem(int id, AddUpdateItemRequestModel request);
        void DeleteItem(ToDoItemModel request);
    }
}
