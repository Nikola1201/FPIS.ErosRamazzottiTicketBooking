
using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="ReservationTicket"/>: ključ, cena i veze ka rezervaciji, zoni i datumu koncerta.
/// </summary>
public class ReservartionTicketEntityTypeConfiguration : IEntityTypeConfiguration<ReservationTicket>
{
    /// <summary>Konfiguriše mapiranje <see cref="ReservationTicket"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="ReservationTicket"/>.</param>
    public void Configure(EntityTypeBuilder<ReservationTicket> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id)
            .ValueGeneratedOnAdd();

        // Foreign key to Reservation
        builder.HasOne<Reservation>()
            .WithMany(r => r.Tickets)
            .HasForeignKey(rt => rt.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key to Zone
        builder.HasOne(rt => rt.Zone)
            .WithMany()
            .HasForeignKey(rt => rt.ZoneId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship to ConcertDate
        builder.HasOne<ConcertDate>()
            .WithMany(cd => cd.Tickets)
            .HasForeignKey(rt => rt.ConcertDateId)
            .OnDelete(DeleteBehavior.Cascade);

        // Price configuration
        builder.Property(rt => rt.Price)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}
