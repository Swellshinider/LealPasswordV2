using LealPasswordV2.Core.Database.Models;
using LealPasswordV2.Core.Database.ResourceAccess.Interfaces;
using Microsoft.Data.Sqlite;

namespace LealPasswordV2.Core.Database.ResourceAccess;

internal class RegisterAccess : IRegisterAccess<Register>
{
    private readonly SqliteConnection _connection;

    public RegisterAccess(SqliteConnection connection)
    {
        _connection = connection
            ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task AddAsync(Register entity)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
            INSERT INTO Registers 
            (
                UserId, 
                EncryptedName, 
                EncryptedUsername, 
                EncryptedPassword, 
                EncryptedDescription, 
                EncryptedTag,
                CreatedAt, 
                UpdatedAt
            )
            VALUES 
            (
                @UserId,
                @EncryptedName,
                @EncryptedUsername,
                @EncryptedPassword,
                @EncryptedDescription,
                @EncryptedTag,
                @CreatedAt,
                @UpdatedAt
            )
        ";

        command.Parameters.AddWithValue("@UserId", entity.UserId);
        command.Parameters.AddWithValue("@EncryptedName", entity.EncryptedName);
        command.Parameters.AddWithValue("@EncryptedUsername", entity.EncryptedUsername);
        command.Parameters.AddWithValue("@EncryptedPassword", entity.EncryptedPassword);
        command.Parameters.AddWithValue("@EncryptedDescription", entity.EncryptedDescription);
        command.Parameters.AddWithValue("@EncryptedTag", entity.EncryptedTag);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

        var result = await command.ExecuteNonQueryAsync();

        if (result != 1)
            throw new InvalidOperationException("Failed to add register.");
    }

    public async Task<IEnumerable<Register>> GetAllByUserId(string userId)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
            SELECT 
                RegisterId, 
                UserId, 
                EncryptedName, 
                EncryptedUsername, 
                EncryptedPassword, 
                EncryptedDescription, 
                EncryptedTag,
                CreatedAt, 
                UpdatedAt
            FROM 
                Registers
            WHERE 
                UserId = @UserId";

        command.Parameters.AddWithValue("@UserId", userId);

        using var reader = await command.ExecuteReaderAsync();

        return Builder.BuildRegisters(reader);
    }

    public async Task UpdateAsync(Register entity)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
            UPDATE Registers 
            SET 
                EncryptedName = @EncryptedName, 
                EncryptedUsername = @EncryptedUsername, 
                EncryptedPassword = @EncryptedPassword, 
                EncryptedDescription = @EncryptedDescription, 
                EncryptedTag = @EncryptedTag,
                UpdatedAt = @UpdatedAt
            WHERE 
                RegisterId = @RegisterId";

        command.Parameters.AddWithValue("@RegisterId", entity.RegisterId);
        command.Parameters.AddWithValue("@EncryptedName", entity.EncryptedName);
        command.Parameters.AddWithValue("@EncryptedUsername", entity.EncryptedUsername);
        command.Parameters.AddWithValue("@EncryptedPassword", entity.EncryptedPassword);
        command.Parameters.AddWithValue("@EncryptedDescription", entity.EncryptedDescription);
        command.Parameters.AddWithValue("@EncryptedTag", entity.EncryptedTag);
        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

        var result = await command.ExecuteNonQueryAsync();

        if (result != 1)
            throw new InvalidOperationException("Failed to update register.");
    }

    public async Task DeleteAsync(string id)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
            DELETE FROM 
                Registers 
            WHERE 
                RegisterId = @RegisterId";

        command.Parameters.AddWithValue("@RegisterId", id);

        var result = await command.ExecuteNonQueryAsync();

        if (result != 1)
            throw new InvalidOperationException("Failed to delete register.");
    }
}