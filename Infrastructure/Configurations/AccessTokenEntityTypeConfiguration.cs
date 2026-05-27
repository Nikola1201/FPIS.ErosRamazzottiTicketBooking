using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="AccessToken"/>: tabela, ključ, vrednost i jedan-prema-jedan veza ka rezervaciji.
/// </summary>
public class AccessTokenEntityTypeConfiguration : IEntityTypeConfiguration<AccessToken>
{
    /// <summary>Konfiguriše mapiranje <see cref="AccessToken"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="AccessToken"/>.</param>
    public void Configure(EntityTypeBuilder<AccessToken> builder)
    {
        builder.ToTable("AccessTokens");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(t => t.Reservation)
            .WithOne(r => r.AccessToken)
            .HasForeignKey<AccessToken>(t => t.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Property(t => t.Value)
            .IsRequired()
            .HasMaxLength(10);
    }

}
