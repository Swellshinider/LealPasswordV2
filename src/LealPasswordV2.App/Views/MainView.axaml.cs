using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using LealPasswordV2.App.ViewModels;
using SkiaSharp;
using System;

namespace LealPasswordV2.App;

public partial class MainView : Window
{
    private readonly LoginPage _loginPage = new();

    // Fixed size for the main window 16:9
    private const int FixedWidth = 1280;
    private const int FixedHeight = 720;

    public MainView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.ContainerPage = _loginPage;

        Width = FixedWidth;
        Height = FixedHeight;
        MinWidth = FixedWidth;
        MinHeight = FixedHeight;
        CanResize = false;
    }

    private MainViewViewModel ViewModel { get; } = new MainViewViewModel();
}