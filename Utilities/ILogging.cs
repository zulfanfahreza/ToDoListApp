namespace ToDoListApp.Utilities
{
    public interface ILogging
    {
        void LogDebug(string source, string message);
        void LogError(string source, string message);
    }
}
