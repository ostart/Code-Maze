using CompanyEmployees.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasData
        (
            new Client
            {
                Id = new Guid("c1f33503-bb38-4fa1-98a0-6cfaf9986797"),
                Name = "External Client's Test Name"
            }
        );
    }
}