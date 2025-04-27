using CompanyEmployees.Application.Notifications;
using LoggingService;
using MediatR;

namespace CompanyEmployees.Application.Handlers;

internal sealed class EmailHandler : INotificationHandler<CompanyDeletedNotification>
{
    private readonly ILoggerManager _logger;

    public EmailHandler(ILoggerManager logger) => _logger = logger;

    public async Task Handle(CompanyDeletedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning($"Delete action for the company with id: {notification.Id} has occurred.");
            
        await Task.CompletedTask;
    }
}