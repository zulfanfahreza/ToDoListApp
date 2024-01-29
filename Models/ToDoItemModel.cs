namespace ToDoListApp.Models
{
    public class ToDoItemModel : BaseEntityModel
    {
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
