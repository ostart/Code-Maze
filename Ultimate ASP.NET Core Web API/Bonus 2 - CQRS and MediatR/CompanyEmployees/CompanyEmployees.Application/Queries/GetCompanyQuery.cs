using MediatR;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Application.Queries;

public sealed record GetCompanyQuery(Guid Id, bool TrackChanges) : IRequest<CompanyDto>;