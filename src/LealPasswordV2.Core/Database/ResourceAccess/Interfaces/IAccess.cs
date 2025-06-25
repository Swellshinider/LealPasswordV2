namespace LealPasswordV2.Core.Database.ResourceAccess.Interfaces;

internal interface IAccess<in T> where T : class
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
}