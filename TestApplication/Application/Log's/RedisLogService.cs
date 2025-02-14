using System.Text;
using TestApplication.Application.Middleware;

namespace CrashTracker.Application.Log_s
{
    public class RedisLogService
    {
        private readonly ILogger<RedisLogService> _logger;
        private readonly string _logFilePath;
        public RedisLogService(ILogger<RedisLogService> logger)
        {
            _logger = logger;
            _logFilePath = $"Log\\log_{DateTime.UtcNow:yyyy-MM-dd}.txt";
        }
        public void LogRedisError(string description, Exception ex)
        {
            var errorLogEntry = new StringBuilder()
                                .AppendLine("Redis-Log")
                                .AppendLine(description)
                .AppendLine($"[{DateTime.UtcNow:HH:mm:ss}] | Redis Error: {ex.Message}")
                .AppendLine($"Stack Trace: {ex.StackTrace}");

            _logger.LogError(errorLogEntry.ToString());
            File.AppendAllText(_logFilePath, errorLogEntry.ToString() + Environment.NewLine);
        }
    }
}
