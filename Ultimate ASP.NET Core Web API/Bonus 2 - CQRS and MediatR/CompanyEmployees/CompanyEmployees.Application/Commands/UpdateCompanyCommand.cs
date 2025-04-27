using MediatR;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Application.Commands;

public sealed record UpdateCompanyCommand(Guid Id, CompanyForUpdateDto Company, bool TrackChanges) : IRequest;