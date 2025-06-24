using LealPasswordV2.Core.Database.Models;
using LealPasswordV2.Core.Database.Repository.Interfaces;
using LealPasswordV2.Core.Database.ResourceAccess;

namespace LealPasswordV2.Core.Database.Repository;

public class UserRepository : IRepository<User>
{
    private bool disposedValue = false;
    private readonly DatabaseContext _context;
    private readonly UserAccess _userAccess;

    public UserRepository()
    {
        _context = new DatabaseContext();
        _userAccess = new UserAccess(_context._connection);
    }

    public async Task AddAsync(User entity)
    {
        var exist = _userAccess.Exists(entity.Username).Result;

        if (exist)
            throw new ValidationException("User already exists.");

        await _userAccess.AddAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        var exist = _userAccess.Exists(id).Result;

        if (!exist)
            throw new ValidationException("User does not exist.");

        await _userAccess.DeleteAsync(id);
    }

    public async Task<User?> GetAsync(object obj)
    {
        if (obj is not User entity)
            throw new ArgumentException("Invalid argument type. Expected User.");

        return await _userAccess.GetByUsername(entity.Username);
    }

    public async Task UpdateAsync(User entity)
    {
        var exist = await _userAccess.Exists(entity.UserId);

        if (!exist)
            throw new InvalidOperationException("User does not exist.");

        await _userAccess.UpdateAsync(entity);
    }

    #region [ Dispose ]
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue && disposing)
        {
            _context.Dispose();
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}