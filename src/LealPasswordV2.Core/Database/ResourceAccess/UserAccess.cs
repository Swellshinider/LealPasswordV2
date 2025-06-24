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

    public async Task<bool> ExistsById(string id)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT COUNT(*) FROM Users WHERE UserId = @UserId";
        command.Parameters.AddWithValue("@UserId", id);

        var count = await command.ExecuteScalarAsync();

        if (count == null)
            return false;

        if (!long.TryParse(count.ToString(), out var q))
            throw new InvalidOperationException("Failed to parse count from database.");

        return q > 0;
    }

    public async Task AddAsync(User entity)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
            INSERT INTO Users 
            (
                UserId,
                Username, 
                MasterPasswordHash, 
                Salt, 
                CreatedAt
            )
            VALUES (
                @UserId,
                @Username, 
                @MasterPasswordHash, 
                @Salt, 
                @CreatedAt
            );";

        command.Parameters.AddWithValue("@UserId", Util.GenerateGuid());
        command.Parameters.AddWithValue("@Username", entity.Username);
        command.Parameters.AddWithValue("@MasterPasswordHash", entity.MasterPasswordHash);
        command.Parameters.AddWithValue("@Salt", entity.Salt);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
        var result = await command.ExecuteNonQueryAsync();

        if (result != 1)
            throw new InvalidOperationException("Failed to add user.");
    }

    public async Task DeleteAsync(string id)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "DELETE FROM Users WHERE UserId = @UserId";

        command.Parameters.AddWithValue("@UserId", id);

        var result = await command.ExecuteNonQueryAsync();

        if (result != 1)
            throw new InvalidOperationException("Failed to delete user.");
    }

    public async Task<User?> GetByUsername(string username)
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
                Username = @Username";

        command.Parameters.AddWithValue("@Username", username);

        using var reader = await command.ExecuteReaderAsync();

        return Builder.BuildUser(reader);
    }

    /// <summary>
    /// Implementation does not make sense in this context, as the UserAccess is not designed to retrieve a user by ID.
    /// </summary>
    public Task<User?> GetAsync(string id) => throw new NotImplementedException();

    public async Task UpdateAsync(User entity)
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

        var result = await command.ExecuteNonQueryAsync();

        if (result != 1)
            throw new InvalidOperationException("Failed to update user.");
    }
}