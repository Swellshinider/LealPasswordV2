using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using LealPasswordV2.App.ViewModels;
using SkiaSharp;
using System;

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

    private void SliderVertices_ValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        var vertices = (int)Math.Clamp(e.NewValue, 3, 10);
        _viewModel.VerticesQuantity = vertices;

        if (_geometricCanvas == null)
            return;

        _geometricCanvas.VerticesQuantity = vertices;
        _geometricCanvas.Reset();
    }

    private void SliderSpeed_ValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        var speed = (int)Math.Clamp(e.NewValue, 1, 5);
        _viewModel.Speed = speed; 
        
        if (_geometricCanvas == null)
            return;

        //_geometricCanvas.AnimationInterval = speed;
        _geometricCanvas.Reset();
    }
}