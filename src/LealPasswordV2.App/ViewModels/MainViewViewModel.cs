using Avalonia.Controls;

namespace LealPasswordV2.App.ViewModels
{
    public class MainViewViewModel : BaseViewModel
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
    }
}