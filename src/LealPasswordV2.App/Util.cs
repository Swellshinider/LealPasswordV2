using Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using MsBox.Avalonia;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LealPasswordV2.App;

public static class Util
{
    public static void SwitchTo(this Window window, Window target)
    {
        window.ShowInTaskbar = false;
        window.IsEnabled = false;
        window.Hide();

        target.ShowInTaskbar = true;
        target.IsEnabled = true;
        target.Show();

        target.Closed += (s, e) => { window.Close(); };
    }

    /// <summary>
    /// Displays a message box with the specified title, message, icon, and button definitions.
    /// </summary>
    public static async Task<string> DisplayMessageBox(this Window? window, string title, string message, Icon icon, List<ButtonDefinition> buttons)
    {
        var messageParams = new MessageBoxCustomParams()
        {
            Icon = icon,
            ContentTitle = title,
            ContentMessage = message,
            ButtonDefinitions = buttons,
            CanResize = false,
            ShowInCenter = true,
            Topmost = true,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
        };

        var result = MessageBoxManager.GetMessageBoxCustom(messageParams);

        return window == null
            ? await result.ShowAsync()
            : await result.ShowWindowDialogAsync(window!);
    }

    /// <summary>
    /// Displays a message box with the specified title and message, using the Info icon and a single "Ok" button.
    /// </summary>
    public static async Task<string> DisplayMessageBox(this Window? window, string title, string message)
        => await DisplayMessageBox(window, title, message, Icon.Info, [new() { Name = "Ok" }]);
}