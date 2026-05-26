using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

public class PromoCodeEntityTypeConfiguration : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Code)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.IsUsed)
            .IsRequired();

    }
}
