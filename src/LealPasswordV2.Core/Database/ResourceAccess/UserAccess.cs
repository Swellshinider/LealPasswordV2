using LealPasswordV2.Core.Database.Models;
using LealPasswordV2.Core.Database.ResourceAccess.Interfaces;
using Microsoft.Data.Sqlite;

namespace LealPasswordV2.Core.Database.ResourceAccess;

internal class UserAccess : IUserAccess<User>
{
    private readonly SqliteConnection _connection;

    public UserAccess(SqliteConnection connection)
    {
        _connection = connection 
            ?? throw new ArgumentNullException(nameof(connection));
    }

    public Task<bool> Exists(string username)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
        command.Parameters.AddWithValue("@Username", username);

        var count = (long)(command.ExecuteScalar() ?? 0);

        return Task.FromResult(count > 0);
    }

    public Task<bool> Exists(int id)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT COUNT(*) FROM Users WHERE UserId = @UserId";
        command.Parameters.AddWithValue("@UserId", id);

        var count = (long)(command.ExecuteScalar() ?? 0);

        return Task.FromResult(count > 0);
    }

    public Task AddAsync(User entity)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
            INSERT INTO Users 
            (
                Username, 
                MasterPasswordHash, 
                Salt, 
                CreatedAt
            )
            VALUES (
                @Username, 
                @MasterPasswordHash, 
                @Salt, 
                @CreatedAt
            );";

        command.Parameters.AddWithValue("@Username", entity.Username);
        command.Parameters.AddWithValue("@MasterPasswordHash", entity.MasterPasswordHash);
        command.Parameters.AddWithValue("@Salt", entity.Salt);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
        var result = command.ExecuteNonQuery();

        if (result != 1)
            throw new InvalidOperationException("Failed to add user.");

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "DELETE FROM Users WHERE UserId = @UserId";

        command.Parameters.AddWithValue("@UserId", id);

        var result = command.ExecuteNonQuery();

        if (result != 1)
            throw new InvalidOperationException("Failed to delete user.");

        return Task.CompletedTask;
    }

    public async Task<User?> GetByUsernamePasswordAsync(string username, string masterPasswordHashed)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
            SELECT 
                UserId, 
                Username, 
                MasterPasswordHash, 
                Salt, 
                CreatedAt 
            FROM 
                Users 
            WHERE 
                Username = @Username AND 
                MasterPasswordHash = @MasterPasswordHash";

        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@MasterPasswordHash", masterPasswordHashed);

        using var reader = await command.ExecuteReaderAsync();

        return Builder.BuildUser(reader);
    }

    /// <summary>
    /// Implementation does not make sense in this context, as the UserAccess is not designed to retrieve a user by ID.
    /// </summary>
    public Task<User?> GetAsync(int id) => throw new NotImplementedException();

    public Task UpdateAsync(User entity)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
            UPDATE 
                Users 
            SET 
                Username = @Username, 
                MasterPasswordHash = @MasterPasswordHash, 
                Salt = @Salt
            WHERE 
                UserId = @UserId";

        command.Parameters.AddWithValue("@UserId", entity.UserId);
        command.Parameters.AddWithValue("@Username", entity.Username);
        command.Parameters.AddWithValue("@MasterPasswordHash", entity.MasterPasswordHash);
        command.Parameters.AddWithValue("@Salt", entity.Salt);

        var result = command.ExecuteNonQuery();

        if (result != 1)
            throw new InvalidOperationException("Failed to update user.");

        return Task.CompletedTask;
    }
}