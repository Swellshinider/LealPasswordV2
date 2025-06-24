namespace LealPasswordV2.Core.Database.ResourceAccess.Interfaces;

internal interface IUserAccess<T> : IAccess<T> where T : class
{
    Task<bool> Exists(string username);
    Task<bool> ExistsById(string id);
    Task<T?> GetByUsername(string username);
}