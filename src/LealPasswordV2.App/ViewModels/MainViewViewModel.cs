using Avalonia.Controls;
using System.ComponentModel;

namespace LealPasswordV2.App.ViewModels
{
    public class MainViewViewModel : INotifyPropertyChanged
    {
        private Control? _containerPage;

        public Control ContainerPage
        {
            get => _containerPage!;
            set
            {
                if (_containerPage != value)
                {
                    _containerPage = value;
                    OnPropertyChange(nameof(ContainerPage));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChange(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}