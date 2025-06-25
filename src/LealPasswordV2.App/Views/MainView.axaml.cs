using Avalonia.Controls;
using LealPasswordV2.App.ViewModels;

namespace LealPasswordV2.App;

public partial class MainView : Window
{
    private readonly LoginPage _loginPage = new();
    private readonly MainViewViewModel _viewModel = new();

    // Fixed size for the main window 16:9
    private const int FixedWidth = 1280;
    private const int FixedHeight = 720;

    public MainView()
    {
        InitializeComponent();
        DataContext = _viewModel;
        _viewModel.ContainerPage = _loginPage;

        Width = FixedWidth;
        Height = FixedHeight;
        MinWidth = FixedWidth;
        MinHeight = FixedHeight;
        CanResize = false;
    }
}