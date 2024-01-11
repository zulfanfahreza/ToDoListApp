using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public interface IToDoService
    {
        List<ToDoItemModel> GetAllItems();
        ToDoItemModel GetById(int id);
        void AddItem(ToDoItemModel item);
        ToDoItemModel UpdateItem(int id, UpdateItemRequestModel request);
        void DeleteItem(ToDoItemModel request);
    }
}
