using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FPIS.Infrastructure.Configurations;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="Concert"/>: ograničenja dužine, obavezna polja i veza ka datumima.
/// </summary>
public class ConcertEntityTypeConfiguration : IEntityTypeConfiguration<Concert>
{
    /// <summary>Konfiguriše mapiranje <see cref="Concert"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="Concert"/>.</param>
    public void Configure(EntityTypeBuilder<Concert> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Venue)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.AdditionalInfo)
            .HasMaxLength(500);

        builder.HasMany(c => c.Dates)
            .WithOne(cd => cd.Concert)
            .HasForeignKey(cd => cd.ConcertId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
