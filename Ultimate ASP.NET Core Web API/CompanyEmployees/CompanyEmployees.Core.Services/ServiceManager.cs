﻿using AutoMapper;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using CompanyEmployees.Core.Domain.ConfigurationModels;

namespace CompanyEmployees.Core.Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IClientService> _clientService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, 
        IMapper mapper, IEmployeeLinks employeeLinks,
        UserManager<User> userManager, IOptionsMonitor<JwtConfiguration> configuration)
    {
        _companyService = new Lazy<ICompanyService>(() => 
            new CompanyService(repositoryManager, logger, mapper));
        _employeeService = new Lazy<IEmployeeService>(() => 
            new EmployeeService(repositoryManager, logger, mapper, employeeLinks));
        _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticationService(logger, mapper, userManager, configuration));
        _clientService = new Lazy<IClientService>(() => new ClientService(repositoryManager, mapper));
    }

    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IClientService ClientService => _clientService.Value;
}