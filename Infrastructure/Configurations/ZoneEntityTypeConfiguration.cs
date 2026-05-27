using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="Zone"/>: ključ, naziv, kapacitet i cena.
/// </summary>
public class ZoneEntityTypeConfiguration : IEntityTypeConfiguration<Zone>
{
    /// <summary>Konfiguriše mapiranje <see cref="Zone"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="Zone"/>.</param>
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
