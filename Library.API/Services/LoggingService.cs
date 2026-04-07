using Library.Core.Interfaces;

namespace Library.API.Services
{
    public class LoggingService: ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public Task LogInfoAsync(string message)
        {
            _logger.LogInformation(message);
            return Task.CompletedTask;
        }

        public async Task LogErrorAsync(string message, Exception? ex = null) {
            _logger.LogWarning(message);
            await Task.CompletedTask;
        }
    }
}
