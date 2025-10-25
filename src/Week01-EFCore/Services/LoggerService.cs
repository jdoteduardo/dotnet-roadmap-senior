using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public Task LogInformationAsync(string message)
        {
            _logger.LogInformation(message);
            return Task.CompletedTask;
        }

        public Task LogWarningAsync(string message)
        {
            _logger.LogWarning(message);
            return Task.CompletedTask;
        }

        public Task LogErrorAsync(string message, Exception? exception = null)
        {
            _logger.LogError(exception, message);
            return Task.CompletedTask;
        }
    }
}