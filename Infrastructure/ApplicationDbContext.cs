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
    public DbSet<Concert> Concerts => Set<Concert>();
    public DbSet<ConcertDate> ConcertDates => Set<ConcertDate>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Discount> Discounts => Set<Discount>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
    public DbSet<ReservationTicket> ReservationTickets => Set<ReservationTicket>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ZoneEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TokenEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PromoCodeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ReservartionTicketEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ConcertEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ConcertDateEntityTypeConfiguration());


        // Seed Zones
        var zone1Id = Guid.NewGuid();
        var zone2Id = Guid.NewGuid();

        modelBuilder.Entity<Zone>().HasData(
            new Zone { Id = zone1Id, Name = "VIP", Capacity = 100, Price = 250.00m },
            new Zone { Id = zone2Id, Name = "Regular", Capacity = 500, Price = 100.00m }
        );

        // Seed Concerts
        var concert1Id = Guid.NewGuid();
        modelBuilder.Entity<Concert>().HasData(
            new Concert
            {
                Id = concert1Id,
                Name = "Eros Ramazzotti Live",
                City = "Rome",
                Venue = "Colosseum",
                Address = "Piazza del Colosseo, 1",
                AdditionalInfo = "Open air concert"
            }
        );

        // Seed ConcertDates
        var concertDate1Id = Guid.NewGuid();
        modelBuilder.Entity<ConcertDate>().HasData(
            new
            {
                Id = concertDate1Id,
                Date = new DateTime(2025, 11, 15,21,0,0),
                ConcertId = concert1Id
            }
        );
    }
}
