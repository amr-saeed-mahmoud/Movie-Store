using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieStore.Models.Data;
using MovieStore.Repos.Abstract;

namespace MovieStore.Repos.Implemention;


public class GenricRepo<T> : IGenricRepo<T> where T : class
{
    private readonly AppDbContext _db;

    // constractor
    public GenricRepo(AppDbContext db)
    {
        _db = db;
    }

    // read operation
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _db.Set<T>().Where(predicate).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _db.Set<T>().FindAsync(id);
    }

    // write operations

    public async Task<bool> AddAsync(T entity)
    {
        await _db.Set<T>().AddAsync(entity);
        int AffectedRows = await _db.SaveChangesAsync();
        return AffectedRows > 0;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if(entity != null)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _db.Set<T>().Update(entity);
        int AffectedRows = await _db.SaveChangesAsync();
        return AffectedRows > 0;
    }
}