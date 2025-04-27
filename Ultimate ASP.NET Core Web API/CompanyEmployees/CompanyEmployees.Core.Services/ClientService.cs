using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Core.Services;

internal sealed class ClientService : IClientService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public ClientService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClientDto>> GetAllClientsAsync(bool trackChanges, CancellationToken ct = default)
    {
        var clients = await _repository.Client.GetAllClientsAsync(trackChanges, ct);

        var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);

        return clientsDto;
    }

    public async Task<ClientDto> CreateClientAsync(ClientForCreationDto client, CancellationToken ct = default)
    {
        var clientEntity = _mapper.Map<Client>(client);

        _repository.Client.CreateClient(clientEntity);
        await _repository.SaveAsync(ct);

        var clientToReturn = _mapper.Map<ClientDto>(clientEntity);

        return clientToReturn;
    }
}