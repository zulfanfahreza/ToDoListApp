namespace ToDoListApp.Models
{
    public class ErrorResponseModel
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
