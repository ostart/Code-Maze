using MediatR;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Application.Queries;

public sealed record GetCompaniesQuery(bool TrackChanges) : IRequest<IEnumerable<CompanyDto>>;