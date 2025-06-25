using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LealPasswordV2.App;

public partial class LoginPage : UserControl
{
    private Window? _window; 
    
    private List<Point> _vertices = [];
    private DispatcherTimer _timer;
    private readonly Random _rand = new();

    public LoginPage()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        var visualRoot = this.GetVisualRoot();
        if (visualRoot != null)
            _window = visualRoot as Window;

        base.OnAttachedToVisualTree(e);
    }
}