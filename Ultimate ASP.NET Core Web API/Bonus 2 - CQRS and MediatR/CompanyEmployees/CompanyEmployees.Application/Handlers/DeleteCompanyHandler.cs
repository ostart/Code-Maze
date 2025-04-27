using CompanyEmployees.Application.Notifications;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.Repositories;
using MediatR;

namespace CompanyEmployees.Application.Handlers;

internal sealed class DeleteCompanyHandler : INotificationHandler<CompanyDeletedNotification>
{
    private readonly IRepositoryManager _repository;

    public DeleteCompanyHandler(IRepositoryManager repository) => _repository = repository;

    public async Task Handle(CompanyDeletedNotification notification, CancellationToken cancellationToken)
    {
        var company = await _repository.Company.GetCompanyAsync(notification.Id, notification.TrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(notification.Id);

        _repository.Company.DeleteCompany(company);
        await _repository.SaveAsync(cancellationToken);
    }
} 