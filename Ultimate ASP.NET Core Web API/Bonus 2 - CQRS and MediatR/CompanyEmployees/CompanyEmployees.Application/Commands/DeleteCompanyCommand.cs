using MediatR;

namespace CompanyEmployees.Application.Commands;

public record DeleteCompanyCommand(Guid Id, bool TrackChanges) : IRequest; 