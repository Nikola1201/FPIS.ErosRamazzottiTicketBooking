using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

public class AppSettingsEntityTypeConfiguration : IEntityTypeConfiguration<AppSettings>
{
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

