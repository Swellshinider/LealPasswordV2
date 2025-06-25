using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LealPasswordV2.App.Controls;

internal class GeometricPatternCanvas : Canvas
{
    public static readonly StyledProperty<int> PointCountProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, int>(nameof(VerticesQuantity), 15);

    public static readonly StyledProperty<bool> AnimatedProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, bool>(nameof(Animated), true);

    // Animation timer interval in milliseconds
    public static readonly StyledProperty<TimeSpan> AnimationIntervalProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, TimeSpan>(nameof(AnimationInterval), TimeSpan.FromMilliseconds(500));

    public int VerticesQuantity
    {
        get => GetValue(PointCountProperty);
        set => SetValue(PointCountProperty, value);
    }

    public bool Animated
    {
        get => GetValue(AnimatedProperty);
        set => SetValue(AnimatedProperty, value);
    }

    public TimeSpan AnimationInterval
    {
        get => GetValue(AnimationIntervalProperty);
        set => SetValue(AnimationIntervalProperty, value);
    }

    private readonly DispatcherTimer _timer;
    private readonly Random _rand = new();
    private List<Point> _points = [];
    private bool _initialized = false; 
    
    private double _pulse = 1.0;
    private double _pulseDirection = -1; // -1 = inward, 1 = outward


    public GeometricPatternCanvas()
    {
        _timer = new DispatcherTimer
        {
            Interval = AnimationInterval
        };

        _timer.Tick += RenderGeometricPattern;
        _timer.Start(); 
        LayoutUpdated += OnLayoutUpdated;
    }

    protected override Size MeasureOverride(Size availableSize) => availableSize;

    protected override Size ArrangeOverride(Size finalSize) => finalSize;

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (_initialized || Bounds.Width == 0 || Bounds.Height == 0)
            return;

        _initialized = true;

        _points = [.. Enumerable.Range(0, VerticesQuantity).Select(_ => new Point(_rand.NextDouble() * Bounds.Width, _rand.NextDouble() * Bounds.Height))];
    }

    private void RenderGeometricPattern(object? sender, EventArgs e)
    {
        if (_points.Count == 0 || Bounds.Width == 0 || Bounds.Height == 0)
            return;

        Children.Clear();



        if (!Animated)
            _timer.Stop();
    }
}