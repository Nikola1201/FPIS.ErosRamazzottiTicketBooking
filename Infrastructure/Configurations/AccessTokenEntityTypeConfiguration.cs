using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

public class AccessTokenEntityTypeConfiguration : IEntityTypeConfiguration<AccessToken>
{
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
