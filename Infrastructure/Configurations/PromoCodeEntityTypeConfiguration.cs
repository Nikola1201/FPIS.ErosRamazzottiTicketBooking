using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="PromoCode"/>: ključ, dužina koda i obavezno polje IsUsed.
/// </summary>
public class PromoCodeEntityTypeConfiguration : IEntityTypeConfiguration<PromoCode>
{
    /// <summary>Konfiguriše mapiranje <see cref="PromoCode"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="PromoCode"/>.</param>
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
