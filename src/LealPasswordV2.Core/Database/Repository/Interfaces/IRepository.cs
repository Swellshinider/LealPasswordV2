namespace LealPasswordV2.Core.Database.Repository.Interfaces;

public interface IRepository<T> : IDisposable where T : class
{
    Task AddAsync(T entity);
    Task<T?> GetAsync(object obj);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}