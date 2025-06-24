using LealPasswordV2.Core.Database.Models;
using Microsoft.Data.Sqlite;

namespace LealPasswordV2.Core.Database.ResourceAccess;

internal static class Builder
{
    internal static User? BuildUser(SqliteDataReader reader)
    {
        if (!reader.HasRows)
            return null;

        reader.Read();

        return new User
        {
            UserId = reader.GetTypedValue<string>("UserId"),
            Username = reader.GetTypedValue<string>("Username"),
            MasterPasswordHash = reader.GetTypedValue<byte[]>("MasterPasswordHash"),
            Salt = reader.GetTypedValue<byte[]>("Salt"),
            CreatedAt = reader.GetTypedValue<DateTime>("CreatedAt")
        };
    }

    internal static IEnumerable<Register> BuildRegisters(SqliteDataReader reader)
    {
        if (!reader.HasRows)
            yield break;

        while (reader.Read())
        {
            yield return new Register
            {
                RegisterId = reader.GetTypedValue<string>("RegisterId"),
                UserId = reader.GetTypedValue<string>("UserId"),
                EncryptedName = reader.GetTypedValue<string>("EncryptedName"),
                EncryptedUsername = reader.GetTypedValue<string>("EncryptedUsername"),
                EncryptedPassword = reader.GetTypedValue<string>("EncryptedPassword"),
                EncryptedDescription = reader.GetTypedValue<string?>("EncryptedDescription"),
                CreatedAt = reader.GetTypedValue<DateTime>("CreatedAt"),
                UpdatedAt = reader.GetTypedValue<DateTime>("UpdatedAt")
            };
        }   
    }
}