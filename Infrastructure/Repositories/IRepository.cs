using FPIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FPIS.Infrastructure.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asNoTracking = false,
        params Expression<Func<TEntity, object>>[] includes);


    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void RemoveRange(ICollection<TEntity> entities);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

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

    public async Task<TEntity?> GetByIdAsync(Guid id)
        => await _dbSet.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);

    public async Task AddAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public void Update(TEntity entity)
        => _dbSet.Update(entity);

    public void Delete(TEntity entity)
        => _dbSet.Remove(entity);

    public void RemoveRange(ICollection<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate != null)
            return _dbSet.CountAsync(predicate);
        return _dbSet.CountAsync();

    }
}
