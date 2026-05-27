using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FPIS.Infrastructure.Configurations;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="Discount"/>: ključ, kolone i veza ka <see cref="Reservation"/>.
/// </summary>
public class DiscountEntityTypeConfiguration : IEntityTypeConfiguration<Discount>
{
    /// <summary>Konfiguriše mapiranje <see cref="Discount"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="Discount"/>.</param>
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd();

        builder.Property(d => d.Type)
            .IsRequired();

        builder.Property(d => d.Percentage)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(d => d.ReservationId)
            .IsRequired();

        builder.HasOne(d => d.Reservation)
            .WithMany(r => r.Discounts)
            .HasForeignKey(d => d.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
