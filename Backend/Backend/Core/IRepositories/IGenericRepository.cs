namespace Backend.Core.IRepositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(string id);
    Task<bool> Add(T entity);
    bool Delete(T entity);
    bool Update(T entity);
    Task<bool> Exists(string id);
}
