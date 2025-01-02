using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Service.Models;

namespace Order.Service.Infrastructure.Data.EntityFramework;

internal class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasKey(op => new { op.OrderId, op.ProductId });
        builder.Property(op => op.ProductId).IsRequired();
        builder.Property(op => op.Quantity).IsRequired();
    }
}