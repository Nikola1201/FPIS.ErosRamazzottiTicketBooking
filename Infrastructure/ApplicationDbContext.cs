using FPIS.Domain.Models;
using FPIS.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FPIS.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Discount> Discounts => Set<Discount>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
    public DbSet<ReservationTicket> ReservationTickets => Set<ReservationTicket>(); // <-- Added

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ZoneEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TokenEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PromoCodeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ReservartionTicketEntityTypeConfiguration()); // <-- Added
    }
}
