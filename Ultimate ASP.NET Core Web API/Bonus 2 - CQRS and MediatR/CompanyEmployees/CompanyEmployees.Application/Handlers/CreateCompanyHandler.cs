using AutoMapper;
using CompanyEmployees.Application.Commands;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using MediatR;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Application.Handlers;

internal sealed class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CreateCompanyHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyEntity = _mapper.Map<Company>(request.Company);

        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync(cancellationToken);

        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

        return companyToReturn;
    }
}