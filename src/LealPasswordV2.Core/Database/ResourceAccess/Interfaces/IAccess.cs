namespace LealPasswordV2.Core.Database.ResourceAccess.Interfaces;

internal interface IAccess<T> where T : class
{
    Task AddAsync(T entity);
    Task<T?> GetAsync(int id);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}