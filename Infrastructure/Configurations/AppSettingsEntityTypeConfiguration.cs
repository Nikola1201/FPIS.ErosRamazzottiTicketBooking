using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

/// <summary>
/// EF Core entity type konfiguracija za <see cref="AppSettings"/>: jedinstven Key i ograničenja dužine.
/// </summary>
public class AppSettingsEntityTypeConfiguration : IEntityTypeConfiguration<AppSettings>
{
    /// <summary>Konfiguriše mapiranje <see cref="AppSettings"/> entiteta na bazu.</summary>
    /// <param name="builder">EF Core builder za <see cref="AppSettings"/>.</param>
    public void Configure(EntityTypeBuilder<AppSettings> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(a => a.Key)
            .IsUnique();

        builder.Property(a => a.Value)
            .IsRequired()
            .HasMaxLength(500);
    }
}

