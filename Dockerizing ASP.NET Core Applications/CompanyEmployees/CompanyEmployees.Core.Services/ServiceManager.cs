﻿using AutoMapper;
using CompanyEmployees.Core.Domain.ConfigurationModels;
using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Core.Domain.Repositories;
using CompanyEmployees.Core.Services.Abstractions;
using LoggingService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CompanyEmployees.Core.Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, 
        IMapper mapper, IEmployeeLinks employeeLinks,
        UserManager<User> userManager, IOptions<JwtConfiguration> configuration)
    {
        _companyService = new Lazy<ICompanyService>(() => 
            new CompanyService(repositoryManager, logger, mapper));
        _employeeService = new Lazy<IEmployeeService>(() => 
            new EmployeeService(repositoryManager, logger, mapper, employeeLinks));
        _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticationService(logger, mapper, userManager, configuration));

    }

    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
