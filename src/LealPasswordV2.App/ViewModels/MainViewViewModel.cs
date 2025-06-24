using System.ComponentModel;

namespace LealPasswordV2.App.ViewModels
{
    public class MainViewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;



        private void OnPropertyChange(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}