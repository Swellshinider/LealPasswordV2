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

    public Task AddAsync(User entity)
    {
        var exist = _userAccess.Exists(entity.Username).Result;

        if (exist)
            return Task.FromException(new InvalidOperationException("User already exists."));

        return _userAccess.AddAsync(entity);
    }

    public Task DeleteAsync(int id)
    {
        var exist = _userAccess.Exists(id).Result;

        if (!exist)
            return Task.FromException(new InvalidOperationException("User does not exist."));

        return _userAccess.DeleteAsync(id);
    }

    public Task<User?> GetAsync(object obj)
    {
        if (obj is not User entity)
            return Task.FromException<User?>(new ArgumentException("Invalid argument type. Expected User."));

        return _userAccess.GetByUsernamePasswordAsync(entity.Username, entity.MasterPasswordHash);
    }

    public Task UpdateAsync(User entity)
    {
        var exist = _userAccess.Exists(entity.UserId).Result;

        if (!exist)
            return Task.FromException(new InvalidOperationException("User does not exist."));

        return _userAccess.UpdateAsync(entity);
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