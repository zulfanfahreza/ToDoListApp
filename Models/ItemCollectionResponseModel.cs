namespace ToDoListApp.Models
{
    public class ItemCollectionResponseModel
    {
        public List<ToDoItemModel> Items { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Sort { get; set; }
        public string SortBy { get; set; }
        public int Limit { get; set; }
    }
}
