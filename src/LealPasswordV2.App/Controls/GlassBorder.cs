using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace LealPasswordV2.App.Controls;

internal class GlassBorder : Border
{
    public static readonly StyledProperty<double> OpacityLevelProperty =
        AvaloniaProperty.Register<GlassBorder, double>(nameof(OpacityLevel), 0.8);

    public double OpacityLevel
    {
        get => GetValue(OpacityLevelProperty);
        set => SetValue(OpacityLevelProperty, value);
    }

    public GlassBorder() { }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        SolidColorBrush baseColor;
        var parentColor = this.FindParentBackground();

        if (parentColor is SolidColorBrush pc)
            baseColor = pc;
        else
            baseColor = new SolidColorBrush(Colors.Black);

        var opacity = (byte)(255 * Math.Clamp(OpacityLevel, 0, 1));

        Background = new SolidColorBrush(new Color(opacity, baseColor.Color.R, baseColor.Color.G, baseColor.Color.B));

        base.OnLoaded(e);
    }
}