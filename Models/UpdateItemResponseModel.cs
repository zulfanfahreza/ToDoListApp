namespace ToDoListApp.Models
{
    public class UpdateItemResponseModel
    {
        public ToDoItemModel Item { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
