using MediatR;
using TestApplication.Core.Interfaces.Crash;

namespace CrashTracker.Core
{
    public class OperationChangedEventHandler : INotificationHandler<OperationChangedEvent>
    {
        private readonly ICrashService _crashService;
        private readonly ILogger<OperationChangedEventHandler> _logger;
        public OperationChangedEventHandler(ICrashService crashService, ILogger<OperationChangedEventHandler> logger)
        {
            _crashService = crashService;
            _logger = logger;
        }

        public async Task Handle(OperationChangedEvent notification, CancellationToken cancellationToken)
        {
            var result = await _crashService.UpdateProgress(notification.CrashId);
            if (result.IsFailure) _logger.LogError($"Не удалось обновить прогресс для аварии {notification.CrashId}");
        }
    }

}
