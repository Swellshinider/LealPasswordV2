using LealPasswordV2.Core.Database.Models;
using LealPasswordV2.Core.Database.Repository.Interfaces;
using LealPasswordV2.Core.Database.ResourceAccess;

namespace LealPasswordV2.Core.Database.Repository;

public class RegisterRepository : IRepository<Register>
{
    private bool disposedValue = false;
    private readonly DatabaseContext _context;
    private readonly RegisterAccess _registerAccess;

    public RegisterRepository()
    {
        _context = new DatabaseContext();
        _registerAccess = new RegisterAccess(_context._connection);
    }

    public async Task AddAsync(Register entity) => await _registerAccess.AddAsync(entity);

    public async Task<Register?> GetAsync(object obj)
    {
        if (obj is string id)
        {
            var registers = await _registerAccess.GetAllByUserId(id);
            return registers.FirstOrDefault();
        }
        else if (obj is Register register)
        {
            var registers = await _registerAccess.GetAllByUserId(register.UserId);
            return registers.FirstOrDefault(r => r.RegisterId == register.RegisterId);
        }

        throw new ValidationException("Invalid argument type. Expected string or Register.");
    }

    public async Task UpdateAsync(Register entity) => await _registerAccess.UpdateAsync(entity);

    public async Task DeleteAsync(string id) => await _registerAccess.DeleteAsync(id);

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