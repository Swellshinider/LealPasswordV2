using Microsoft.Data.Sqlite;

namespace LealPasswordV2.Core.Database;

internal sealed class DatabaseContext : IDisposable
{
    private bool disposedValue = false;
    internal readonly SqliteConnection _connection;

    internal DatabaseContext()
    {
        var directoryPath = Path.Combine(Configuration.BaseDirectory, "Data");
        var filePath = Path.Combine(directoryPath, $"{Configuration.DatabaseName}.db");

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        if (!File.Exists(filePath))
            File.Create(filePath).Dispose();

        _connection = new($"Data Source={filePath}");
        _connection.Open();
        Initialize();
    }

    internal DatabaseContext(SqliteConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _connection.Open();
        Initialize();
    }

    private void Initialize()
    {
        if (Configuration.DatabaseCreated)
            return;

        using var command = _connection.CreateCommand();

        // Create tables
        command.CommandText = $@"
            CREATE TABLE IF NOT EXISTS Users (
                UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                MasterPasswordHash TEXT NOT NULL,
                Salt TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS PasswordRegisters (
                RegisterId INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Username TEXT NOT NULL,
                EncryptedPassword TEXT NOT NULL,
                Description TEXT,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS CreditCards (
                CardId INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                CardholderName TEXT NOT NULL,
                CardNumberEncrypted TEXT NOT NULL,
                ExpirationDateEncrypted TEXT NOT NULL,
                CVVEncrypted TEXT NOT NULL,
                Brand TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS SecretNotes (
                NoteId INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                Title TEXT NOT NULL,
                EncryptedNote TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
            );
        ";

        command.ExecuteNonQuery();
        Configuration.DatabaseCreated = true;
    }

    #region [ Dispose ]
    private void Dispose(bool disposing)
    {
        if (!disposedValue && disposing)
        {
            _connection.Close();
            _connection.Dispose();
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