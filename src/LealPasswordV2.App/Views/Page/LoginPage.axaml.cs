using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using LealPasswordV2.App.ViewModels;

namespace LealPasswordV2.App;

public partial class LoginPage : UserControl
{
    public delegate void SignUpEventHandler();
    public event SignUpEventHandler? SignUpClicked;

    public delegate void LoginEventHandler(string username, string password);
    public event LoginEventHandler? LogInClicked;

    public LoginPage()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private LoginPageViewModel ViewModel { get; } = new LoginPageViewModel();

    private void SignUp_Click(object? sender, PointerPressedEventArgs e)
        => SignUpClicked?.Invoke();

    private void ShowPassword_Checked(object? sender, RoutedEventArgs e)
        => PasswordBox.PasswordChar = '\0';

    private void HidePassword_Checked(object? sender, RoutedEventArgs e)
        => PasswordBox.PasswordChar = '*';

    private void LogIn_Click(object? sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        var password = PasswordBox.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            // Handle empty fields, e.g., show a message to the user
            return;
        }

        LogInClicked?.Invoke(username, password);
    }
}