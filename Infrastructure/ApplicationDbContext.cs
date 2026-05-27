using FPIS.Domain.Models;
using FPIS.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FPIS.Infrastructure;

/// <summary>
/// EF Core DbContext za FPIS aplikaciju; sadrži DbSet-ove za sve perzistentne entitete i primenjuje entity konfiguracije.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>Inicijalizuje DbContext sa datim opcijama (connection string, provider).</summary>
    /// <param name="options">EF Core opcije konfigurisane u Program.cs.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    /// <summary>Tabela konfiguracionih parametara aplikacije.</summary>
    public DbSet<AppSettings> AppSettings => Set<AppSettings>();
    /// <summary>Tabela koncerata.</summary>
    public DbSet<Concert> Concerts => Set<Concert>();
    /// <summary>Tabela datuma koncerata.</summary>
    public DbSet<ConcertDate> ConcertDates => Set<ConcertDate>();
    /// <summary>Tabela kupaca.</summary>
    public DbSet<Customer> Customers => Set<Customer>();
    /// <summary>Tabela popusta primenjenih na rezervacije.</summary>
    public DbSet<Discount> Discounts => Set<Discount>();
    /// <summary>Tabela rezervacija.</summary>
    public DbSet<Reservation> Reservations => Set<Reservation>();
    /// <summary>Tabela zona (kategorija mesta).</summary>
    public DbSet<Zone> Zones => Set<Zone>();
    /// <summary>Tabela pristupnih tokena za rezervacije.</summary>
    public DbSet<AccessToken> Tokens => Set<AccessToken>();
    /// <summary>Tabela promo kodova.</summary>
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
    /// <summary>Tabela karata u rezervacijama.</summary>
    public DbSet<ReservationTicket> ReservationTickets => Set<ReservationTicket>();
    /// <summary>Primenjuje entity konfiguracije na model i seed-uje početne podatke (zone, koncert, datumi, AppSettings).</summary>
    /// <param name="modelBuilder">EF Core <see cref="ModelBuilder"/>.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ZoneEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessTokenEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PromoCodeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ReservartionTicketEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ConcertEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ConcertDateEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AppSettingsEntityTypeConfiguration());

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
                Date = new DateTime(2027, 11, 15, 21, 0, 0),
                ConcertId = concert1Id
            },
            new
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(2026, 11, 17, 21, 0, 0),
                ConcertId = concert1Id,
            },
            new
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(2025, 11, 19, 21, 0, 0),
                ConcertId = concert1Id
            }
        );

        // Seed AppSettings
        modelBuilder.Entity<AppSettings>().HasData(
            new()
            {
                Id = Guid.NewGuid(),
                Key = "EarlyBirdDiscountPercentage",
                Value = "10"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "EarlyBirdDiscountDaysBefore",
                Value = "60"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "FifthTicketDiscountPercentage",
                Value = "50"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "FriendPromoDiscountPercentage",
                Value = "5"
            }
        );
    }
}
