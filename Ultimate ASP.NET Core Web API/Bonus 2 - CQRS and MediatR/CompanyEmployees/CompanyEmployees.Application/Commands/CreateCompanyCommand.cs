using MediatR;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Application.Commands;

public sealed record CreateCompanyCommand(CompanyForCreationDto Company) : IRequest<CompanyDto>;