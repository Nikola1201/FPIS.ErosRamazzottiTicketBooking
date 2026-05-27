using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="Reservation"/>: ključ, status, veze ka kupcu, tokenu, promo kodovima, popustima i kartama.
/// </summary>
public class ReservationEntityTypeConfiguration : IEntityTypeConfiguration<Reservation>
{
    /// <summary>Konfiguriše mapiranje <see cref="Reservation"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="Reservation"/>.</param>
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Status)
            .IsRequired();

        builder.HasOne(r => r.Customer)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.AccessToken)
            .WithOne(t => t.Reservation)
            .IsRequired()
            .HasForeignKey<AccessToken>(t => t.ReservationId);

        builder.HasOne(r => r.UsedPromoCode)
              .WithOne(p => p.UsedByReservation)
              .HasForeignKey<PromoCode>(p => p.UsedByReservationId)
              .OnDelete(DeleteBehavior.Restrict);


        // Reservation GENERATED a promo code (one-to-one)
        builder.HasOne(r => r.GeneratedPromoCode)
            .WithOne(p => p.GeneratedByReservation)
            .HasForeignKey<PromoCode>(p => p.GeneratedByReservationId)
            .OnDelete(DeleteBehavior.SetNull);


        builder.HasMany(r => r.Discounts)
            .WithOne(d => d.Reservation)
            .HasForeignKey(d => d.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Tickets)
            .WithOne()
            .HasForeignKey(t => t.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
