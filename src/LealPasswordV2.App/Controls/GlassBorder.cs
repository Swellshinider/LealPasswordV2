using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Linq;

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
        base.OnLoaded(e);
        LoadShadowEffect();
        LoadBackgroundTransparency();
    }

    private void LoadShadowEffect()
    {
        Effect = new DropShadowEffect()
        {
            Opacity = Math.Clamp(OpacityLevel, 0, 1),
            BlurRadius = 8,
            OffsetX = 0,
            OffsetY = 0,
        };
        ClipToBounds = true;
    }

    private void LoadBackgroundTransparency()
    {
        SolidColorBrush baseColor;
        var parentColor = this.FindParentBackground();
        var opacity = (byte)(255 * Math.Clamp(OpacityLevel, 0, 1));

        if (parentColor is null)
            baseColor = new SolidColorBrush(Colors.Black);
        else
            baseColor = parentColor.ToSolidColorBrush();

        Background = new SolidColorBrush(new Color(opacity, baseColor.Color.R, baseColor.Color.G, baseColor.Color.B));
    }
}