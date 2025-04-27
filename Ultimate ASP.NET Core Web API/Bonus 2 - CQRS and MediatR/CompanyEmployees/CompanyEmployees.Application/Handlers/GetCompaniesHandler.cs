﻿using AutoMapper;
using CompanyEmployees.Application.Queries;
using CompanyEmployees.Core.Domain.Repositories;
using MediatR;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Application.Handlers;

internal sealed class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetCompaniesHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto>> Handle(GetCompaniesQuery request, 
        CancellationToken cancellationToken)
    {
        var companies = await _repository.Company.GetAllCompaniesAsync(request.TrackChanges);

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }
}