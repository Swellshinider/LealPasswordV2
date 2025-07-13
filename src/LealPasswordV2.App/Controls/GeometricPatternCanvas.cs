using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LealPasswordV2.App.Controls;

internal class GeometricPatternCanvas : Canvas
{
    public static readonly StyledProperty<int> PointCountProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, int>(nameof(VerticesQuantity), 10);

    public static readonly StyledProperty<bool> AnimatedProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, bool>(nameof(IsAnimated), true);

    public static readonly StyledProperty<long> AnimationIntervalProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, long>(nameof(AnimationInterval), 50);

    public static readonly StyledProperty<Color> LineColorProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, Color>(nameof(LineColor), Colors.Gray);

    public static readonly StyledProperty<bool> IsRegularPolygonProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, bool>(nameof(IsRegularPolygon), true);

    public static readonly StyledProperty<Color> BackgroundColorProperty =
        AvaloniaProperty.Register<GeometricPatternCanvas, Color>(nameof(Background), Colors.Transparent);

    public int VerticesQuantity
    {
        get => GetValue(PointCountProperty);
        set => SetValue(PointCountProperty, value);
    }

    public bool IsAnimated
    {
        get => GetValue(AnimatedProperty);
        set => SetValue(AnimatedProperty, value);
    }

    public long AnimationInterval
    {
        get => GetValue(AnimationIntervalProperty);
        set => SetValue(AnimationIntervalProperty, value);
    }

    public Color LineColor
    {
        get => GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }

    public bool IsRegularPolygon
    {
        get => GetValue(IsRegularPolygonProperty);
        set
        {
            _initialized = false; // Reset initialization to recalculate vertices
            _timer.Stop(); // Stop the timer to prevent rendering
            SetValue(IsRegularPolygonProperty, value);
        }
    }

    public Color BackgroundColor
    {
        get => GetValue(BackgroundColorProperty);
        set
        {
            SetValue(BackgroundColorProperty, value);
            Background = new SolidColorBrush(value);
        }
    }

    private Point _center;
    private double _angle = 0;
    private bool _initialized = false;
    private List<Point> _vertices = [];

    private readonly Random _rand = new();
    private readonly DispatcherTimer _timer;

    public GeometricPatternCanvas()
    {
        _timer = new DispatcherTimer()
        {
            IsEnabled = true,
            Interval = TimeSpan.FromMilliseconds(AnimationInterval),
            Tag = "LealPasswordV2_GeometricPatternCanvasTimer"
        };
        _timer.Tick += RenderGeometricPattern;
        LayoutUpdated += OnLayoutUpdated;
    }

    public void Reset()
    {
        _initialized = false;
        _angle = 0;
        _timer.Stop();
        _vertices.Clear();
        Children.Clear();
        Initialize();
    }

    private void Initialize()
    {
        if (_initialized || Bounds.Width == 0 || Bounds.Height == 0)
            return;

        _initialized = true;
        _vertices.Clear();
        _center = new Point(Bounds.Width / 2, Bounds.Height / 2);

        if (IsRegularPolygon)
        {
            var radius = Math.Min(Bounds.Width, Bounds.Height) / 2;

            for (var i = 0; i < VerticesQuantity; i++)
            {
                var angle = 2 * Math.PI * i / VerticesQuantity;
                var x = _center.X + radius * Math.Cos(angle);
                var y = _center.Y + radius * Math.Sin(angle);
                _vertices.Add(new Point(x, y));
            }
        }
        else
        {
            _vertices = [.. Enumerable.Range(0, VerticesQuantity)
                                .Select(_ => new Point(Math.Clamp(_rand.NextDouble() * Bounds.Width, 0, Bounds.Width),
                                                       Math.Clamp(_rand.NextDouble() * Bounds.Height, 0, Bounds.Height)))];
        }

        _timer.Start();
    }

    private void OnLayoutUpdated(object? sender, EventArgs e) => Initialize();

    private void RenderGeometricPattern(object? sender, EventArgs e)
    {
        if (_vertices.Count == 0 || Bounds.Width == 0 || Bounds.Height == 0)
            return;

        if (_angle >= 360)
            _angle = 0;

        var rotated = _vertices.Select(p =>
        {
            var rotatedPoint = Rotate(p, _center, _angle);
            var newX = Math.Clamp(rotatedPoint.X, 0, Bounds.Width);
            var newY = Math.Clamp(rotatedPoint.Y, 0, Bounds.Height);

            return new Point(newX, newY);

        }).ToList();

        Children.Clear();

        var stroke = new SolidColorBrush(LineColor);

        for (var i = 0; i < rotated.Count; i++)
        {
            var start = rotated[i];
            var end = rotated[(i + 1) % rotated.Count];

            Children.Add(new Line
            {
                StartPoint = start,
                EndPoint = end,
                Stroke = stroke,
                StrokeThickness = 1
            });

            for (var j = i + 1; j < rotated.Count; j++)
            {
                Children.Add(new Line
                {
                    StartPoint = rotated[i],
                    EndPoint = rotated[j],
                    Stroke = stroke,
                    StrokeThickness = 0.4
                });
            }
        }

        _angle += 0.5; // degrees per frame

        if (!IsAnimated)
            _timer.Stop();
    }

    private static Point Rotate(Point point, Point center, double angleDegrees)
    {
        var angle = Math.PI * angleDegrees / 180.0;
        var cos = Math.Cos(angle);
        var sin = Math.Sin(angle);
        var dx = point.X - center.X;
        var dy = point.Y - center.Y;

        return new Point(
            center.X + (dx * cos - dy * sin),
            center.Y + (dx * sin + dy * cos)
        );
    }
}