namespace ToDoListApp.Models
{
    public class BaseEntityModel
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
