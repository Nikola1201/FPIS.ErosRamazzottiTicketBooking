using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FPIS.Infrastructure.Repositories;

/// <summary>
/// Generički repozitorijum sa standardnim CRUD i query operacijama nad entitetom <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">Tip entiteta.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>Vraća sve entitete koji odgovaraju opcionom filteru, sa opcionim include-ovima.</summary>
    /// <param name="predicate">Filter izraz; ako je null, vraćaju se svi.</param>
    /// <param name="asNoTracking">Ako je true, EF ne prati promene (read-only optimizacija).</param>
    /// <param name="includes">Lista include izraza za navigation properties.</param>
    /// <returns>Listu entiteta.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asNoTracking = false,
        params Expression<Func<TEntity, object>>[] includes);


    /// <summary>Vraća entitet po ID-u ili null ako ne postoji.</summary>
    /// <param name="id">Identifikator entiteta.</param>
    /// <returns>Entitet ili null.</returns>
    Task<TEntity?> GetByIdAsync(Guid id);
    /// <summary>Dodaje novi entitet u kontekst.</summary>
    /// <param name="entity">Entitet za dodavanje.</param>
    /// <returns>Zadatak koji se završava kada je entitet dodat.</returns>
    Task AddAsync(TEntity entity);
    /// <summary>Označava entitet kao izmenjen u kontekstu.</summary>
    /// <param name="entity">Entitet za izmenu.</param>
    void Update(TEntity entity);
    /// <summary>Briše entitet iz konteksta.</summary>
    /// <param name="entity">Entitet za brisanje.</param>
    void Delete(TEntity entity);
    /// <summary>Briše više entiteta iz konteksta.</summary>
    /// <param name="entities">Kolekcija entiteta za brisanje.</param>
    void RemoveRange(ICollection<TEntity> entities);
    /// <summary>Vraća broj entiteta koji odgovaraju opcionom filteru.</summary>
    /// <param name="predicate">Filter izraz; ako je null, vraća se ukupan broj.</param>
    /// <returns>Broj entiteta.</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}

/// <summary>
/// EF Core implementacija <see cref="IRepository{TEntity}"/> nad <see cref="ApplicationDbContext"/>.
/// </summary>
/// <typeparam name="TEntity">Tip entiteta.</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    /// <summary>EF Core kontekst aplikacije.</summary>
    protected readonly ApplicationDbContext _context;
    /// <summary>DbSet entiteta nad kojim repozitorijum operiše.</summary>
    protected readonly DbSet<TEntity> _dbSet;

    /// <summary>Konstruktor sa injektovanim <see cref="ApplicationDbContext"/>.</summary>
    /// <param name="context">EF Core kontekst aplikacije.</param>
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asNoTracking = false,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (asNoTracking)
            query = query.AsNoTracking();
        else
        {
            query = query.AsTracking();
        }
        foreach (var include in includes)
            query = query.Include(include);

        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdAsync(Guid id)
        => await _dbSet.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);

    /// <inheritdoc />
    public async Task AddAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    /// <inheritdoc />
    public void Update(TEntity entity)
        => _dbSet.Update(entity);

    /// <inheritdoc />
    public void Delete(TEntity entity)
        => _dbSet.Remove(entity);

    /// <inheritdoc />
    public void RemoveRange(ICollection<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    /// <inheritdoc />
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate != null)
            return _dbSet.CountAsync(predicate);
        return _dbSet.CountAsync();

    }
}
