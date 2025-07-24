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
                    throw new ValidationException("Username cannot be empty.");

                _username = value;
                OnPropertyChange(nameof(Username));
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
                    throw new ValidationException("Password cannot be empty.");

                _password = value;
                OnPropertyChange(nameof(Password));
            }
        }
    }
}