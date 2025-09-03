using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

public class ReservationEntityTypeConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Status)
            .IsRequired();

        builder.HasOne(r => r.Customer)
            .WithMany()
            .IsRequired();

        builder.HasOne(r => r.Token)
            .WithMany()
            .IsRequired();

        builder.HasOne(r => r.PromoCode)
            .WithMany();

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
