namespace LealPasswordV2.Core.Database.ResourceAccess.Interfaces;

internal interface IUserAccess<T> : IAccess<T> where T : class
{
    Task<bool> Exists(string username);
    Task<bool> Exists(int id);
    Task<T?> GetByUsernamePasswordAsync(string username, string masterPasswordHashed);
}