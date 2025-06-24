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
            UserId = reader.GetTypedValue<int>("UserId"),
            Username = reader.GetTypedValue<string>("Username"),
            MasterPasswordHash = reader.GetTypedValue<string>("MasterPasswordHash"),
            Salt = reader.GetTypedValue<string>("Salt"),
            CreatedAt = reader.GetTypedValue<DateTime>("CreatedAt")
        };
    }
}