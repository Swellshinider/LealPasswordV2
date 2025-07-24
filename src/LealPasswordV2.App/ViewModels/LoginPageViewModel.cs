using System.ComponentModel.DataAnnotations;

namespace LealPasswordV2.App.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string? _username;
        private string? _password;

        public string Username
        {
            get => _username ?? "";
            set
            {
                if (_username == value)
                    return;

                if (string.IsNullOrWhiteSpace(value))
                    AddError(nameof(Username), "Username cannot be empty.");
                else
                {
                    _username = value;
                    ClearErrors(nameof(Username));
                    OnPropertyChange(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => _password ?? "";
            set
            {
                if (_password == value)
                    return;

                if (string.IsNullOrWhiteSpace(value))
                    AddError(nameof(Password), "Password cannot be empty.");
                else
                {
                    _password = value;
                    ClearErrors(nameof(Password));
                    OnPropertyChange(nameof(Password));
                }
            }
        }
    }
}