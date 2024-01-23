namespace ToDoListApp.Utilities
{
    public class Logging : ILogging
    {
        private readonly ILogger<Logging> _logger;

        public Logging(ILogger<Logging> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void LogDebug(string source, string message)
        {
            string formattedDate = SetFormattedDate(DateTime.Now);
            _logger?.LogDebug($"Debug: [{formattedDate}] {source} - {message}");
        }

        public void LogError(string source, string message)
        {
            var formattedDate = SetFormattedDate(DateTime.Now);
            _logger?.LogError($"Error: [{formattedDate}] {source} - {message}");
        }

        private string SetFormattedDate(DateTime dateTime)
        {
            var formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return formattedDate;
        }
    }
}
