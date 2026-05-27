
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;

namespace FPIS.Infrastructure.Repositories;

/// <summary>
/// Unit of Work apstrakcija: pristup repozitorijumima po tipu entiteta, transakcijama i čuvanje promena nad DbContext-om.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>Pokreće novu EF Core transakciju.</summary>
    /// <returns>Aktivna <see cref="IDbContextTransaction"/>.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();
    /// <summary>Vraća (kreira pri prvom pristupu) repozitorijum za dati tip entiteta.</summary>
    /// <typeparam name="TEntity">Tip entiteta.</typeparam>
    /// <returns>Repozitorijum tipa <see cref="IRepository{TEntity}"/>.</returns>
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    /// <summary>Čuva sve promene nad DbContext-om u bazu.</summary>
    /// <returns>Broj zapisa koji su izmenjeni u bazi.</returns>
    Task<int> SaveChangesAsync();
}

/// <summary>
/// Implementacija <see cref="IUnitOfWork"/> nad <see cref="ApplicationDbContext"/>; keš-ira repozitorijume po tipu.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    /// <summary>Konstruktor sa injektovanim <see cref="ApplicationDbContext"/>.</summary>
    /// <param name="context">EF Core kontekst aplikacije.</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (!_repositories.TryGetValue(type, out var repository))
        {
            repository = new Repository<TEntity>(_context);
            _repositories[type] = repository;
        }
        return (IRepository<TEntity>)repository;

    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
    /// <inheritdoc />
    public Task<IDbContextTransaction> BeginTransactionAsync() => _context.Database.BeginTransactionAsync();

    /// <summary>Oslobađa interni <see cref="ApplicationDbContext"/>.</summary>
    public void Dispose()
        => _context.Dispose();

}
