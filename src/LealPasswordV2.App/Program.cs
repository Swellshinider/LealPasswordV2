using Avalonia;
using MsBox.Avalonia;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LealPasswordV2.App;

public static class Program
{
    private static Mutex? _mutex;
    private static readonly string MutexName = "LealPasswordV2.App.Mutex";

    [STAThread]
    public static async Task<int> Main(string[] args)
    {
        _mutex = new Mutex(true, MutexName, out var createdNew);

        // If the mutex already exists, it means another instance is running.
        if (!createdNew)
        {
            _ = MessageBoxManager.GetMessageBoxStandard("LealPassword V2", 
                "Another instance of the application is already running.");
            return 0;
        }

        var logFilePath = Path.Combine(Core.Configuration.BaseDirectory, "Logs", $"{Core.Configuration.AppName}_.log");
        var loggerConfiguration = new LoggerConfiguration().WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day);

#if DEBUG
        loggerConfiguration.MinimumLevel.Debug();
        Log.Logger = loggerConfiguration.CreateLogger();
        Log.Debug("Debug mode is enabled. Logging at Debug level.");
#else
        loggerConfiguration.MinimumLevel.Information();
        Log.Logger = loggerConfiguration.CreateLogger();
#endif

        Log.Information("Application started...");

        try
        {
            Log.Debug("Application arguments: {Args}", string.Join(", ", args));
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Something went wrong...");
            return -1;
        }
        finally
        {
            Log.Information("Application is shutting down...");
            _mutex?.ReleaseMutex();
            await Log.CloseAndFlushAsync();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont();
}