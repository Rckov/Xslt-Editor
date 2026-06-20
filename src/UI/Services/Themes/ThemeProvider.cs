using System.Collections.Frozen;
using XsltEditor.Models;
using XsltEditor.Services.Abstractions.Themes;

namespace XsltEditor.Services.Themes;

public class ThemeProvider : IThemeProvider
{
    private static readonly FrozenDictionary<ThemeType, ThemeDescriptor> Themes =
        new Dictionary<ThemeType, ThemeDescriptor>
        {
            [ThemeType.Default] = new(ThemeType.Default,
                "pack://application:,,,/Resources/Themes/Brushes/DefaultBrushes.xaml",
                "pack://application:,,,/Resources/Themes/DefaultHighlighting.xshd"),

            [ThemeType.Dark] = new(ThemeType.Dark,
                "pack://application:,,,/Resources/Themes/Brushes/DarkBrushes.xaml",
                "pack://application:,,,/Resources/Themes/DarkHighlighting.xshd")
        }.ToFrozenDictionary();

    public ThemeDescriptor Get(ThemeType type)
    {
        return Themes.TryGetValue(type, out var descriptor)
            ? descriptor
            : throw new InvalidOperationException($"Theme '{type}' is not registered.");
    }
}