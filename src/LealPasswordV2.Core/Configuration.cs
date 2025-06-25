namespace LealPasswordV2.Core;

public static class Configuration
{
    private static string? _baseDirectory = null;

    internal static bool DatabaseCreated { get; set; } = false;

    public static string AppName => "LealPasswordV2";

    /// <summary>
    /// Gets the base directory for storing configuration files.
    /// </summary>
    public static string BaseDirectory
    {
        get
        {
            if (_baseDirectory == null)
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                _baseDirectory = Path.Combine(folder, AppName);

                if (!Directory.Exists(_baseDirectory))
                    Directory.CreateDirectory(_baseDirectory);
#if DEBUG
                Console.WriteLine(_baseDirectory);
#endif
            }

            return _baseDirectory;
        }
    }
}