using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

public class ZoneEntityTypeConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.HasKey(z => z.Id);
        builder.Property(z => z.Id)
            .ValueGeneratedOnAdd();

        builder.Property(z => z.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(z => z.Capacity)
            .IsRequired();

        builder.Property(z => z.Price)
           .IsRequired()
           .HasPrecision(18, 2);
    }
}
