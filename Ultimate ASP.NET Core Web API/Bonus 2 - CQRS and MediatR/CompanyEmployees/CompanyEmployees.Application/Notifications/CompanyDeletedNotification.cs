using MediatR;

namespace CompanyEmployees.Application.Notifications;

public sealed record CompanyDeletedNotification(Guid Id, bool TrackChanges) : INotification;