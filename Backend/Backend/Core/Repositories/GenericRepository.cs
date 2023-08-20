using Backend.Core.IRepositories;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Repositories;


public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected ApplicationDbContext context;
    protected DbSet<T> dbSet;

    public GenericRepository(ApplicationDbContext _context)
    {
        context = _context;
        dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    public virtual async Task<T> GetById(string id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Add(T entity)
    {
        await dbSet.AddAsync(entity);
        return true;
    }

    public virtual bool Delete(T entity)
    {
        dbSet.Remove(entity);
        return true;
    }

    public virtual bool Update(T entity)
    {
        dbSet.Update(entity);
        return true;
    }

    public virtual async Task<bool> Exists(string id)
    {
        var entity = await dbSet.FindAsync(id);
        return entity != null;
    }
}