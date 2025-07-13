using Avalonia.Controls;
using System.ComponentModel;

namespace LealPasswordV2.App.ViewModels
{
    public class MainViewViewModel : INotifyPropertyChanged
    {
        private Control? _containerPage;
        private int _verticesQuantity = 3;
        private int _speed = 1;
        private string _verticesQuantityText = "Vertices: 3";
        private string _speedText = "Speed: 1";

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

        public int VerticesQuantity
        {
            get => _verticesQuantity;
            set
            {
                if (_verticesQuantity != value)
                {
                    _verticesQuantity = value;
                    VerticesQuantityText = $"Vertices: {value}";
                    OnPropertyChange(nameof(VerticesQuantity));
                }
            }
        }

        public int Speed
        {
            get => _speed;
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    SpeedText = $"Speed: {value}";
                    OnPropertyChange(nameof(Speed));
                }
            }
        }

        public string VerticesQuantityText
        {
            get => _verticesQuantityText;
            set
            {
                if (_verticesQuantityText != value)
                {
                    _verticesQuantityText = value;
                    OnPropertyChange(nameof(VerticesQuantityText));
                }
            }
        }

        public string SpeedText
        {
            get => _speedText;
            set
            {
                if (_speedText != value)
                {
                    _speedText = value;
                    OnPropertyChange(nameof(SpeedText));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChange(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}