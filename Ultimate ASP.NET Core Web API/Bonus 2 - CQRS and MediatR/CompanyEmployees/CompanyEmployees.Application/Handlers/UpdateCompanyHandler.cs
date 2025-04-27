using AutoMapper;
using CompanyEmployees.Application.Commands;
using CompanyEmployees.Core.Domain.Exceptions;
using CompanyEmployees.Core.Domain.Repositories;
using MediatR;

namespace CompanyEmployees.Application.Handlers;

internal sealed class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public UpdateCompanyHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyEntity = await _repository.Company
            .GetCompanyAsync(request.Id, request.TrackChanges);
        if (companyEntity is null)
            throw new CompanyNotFoundException(request.Id);

        _mapper.Map(request.Company, companyEntity);
        await _repository.SaveAsync(cancellationToken);
    }
}