using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Infrastructure.Persistence;

public class ExternalClientContext : DbContext
{
    public ExternalClientContext(DbContextOptions<ExternalClientContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
    }

    public DbSet<Client> Clients { get; set; }
}