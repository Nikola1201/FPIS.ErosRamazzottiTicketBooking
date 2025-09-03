using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FPIS.Infrastructure.Configurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ConfirmedEmail)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Address)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Country)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.City)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.PostalCode)
            .HasMaxLength(50)
            .IsRequired();

        // Optional properties
        builder.Property(c => c.Address2)
            .HasMaxLength(100);

        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(50);
    }
}