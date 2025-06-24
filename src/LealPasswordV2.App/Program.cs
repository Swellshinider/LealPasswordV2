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

        var logFilePath = Path.Combine(Core.Configuration.BaseDirectory, "logs", "app.log");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Application starting...");

        try
        {
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Something went wrong...");
            return -1;
        }
        finally
        {
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