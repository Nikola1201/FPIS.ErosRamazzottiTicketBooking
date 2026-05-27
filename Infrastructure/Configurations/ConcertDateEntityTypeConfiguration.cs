using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FPIS.Infrastructure.Configurations;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="ConcertDate"/>: ključ i veze ka <see cref="Concert"/> i kartama.
/// </summary>
public class ConcertDateEntityTypeConfiguration : IEntityTypeConfiguration<ConcertDate>
{
    /// <summary>Konfiguriše mapiranje <see cref="ConcertDate"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="ConcertDate"/>.</param>
    public void Configure(EntityTypeBuilder<ConcertDate> builder)
    {
        builder.HasKey(cd => cd.Id);

        builder.Property(cd => cd.Id)
            .ValueGeneratedOnAdd();

        builder.Property(cd => cd.Date)
            .IsRequired();

        builder.Property(cd => cd.ConcertId)
            .IsRequired();

        builder.HasOne(cd => cd.Concert)
            .WithMany(c => c.Dates)
            .HasForeignKey(cd => cd.ConcertId)
            .OnDelete(DeleteBehavior.Cascade);

         builder.HasMany(cd => cd.Tickets)
             .WithOne()
             .HasForeignKey("ConcertDateId")
             .OnDelete(DeleteBehavior.Cascade);
    }
}
