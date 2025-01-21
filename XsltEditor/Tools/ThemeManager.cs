using System.Windows;

namespace XsltEditor.Tools;

internal class ThemeManager
{
    private static readonly Dictionary<ThemeType, string> _themePaths = new()
    {
        { ThemeType.Dark, "Resources/Themes/DarkBrushes.xaml" },
        { ThemeType.Light, "Resources/Themes/LightBrushes.xaml" }
    };

    public static ThemeType CurrentTheme { get; private set; } = ThemeType.Light;

    public static event Action<ThemeType>? ThemeChanged;

    public static void Apply(ThemeType themeType)
    {
        if (CurrentTheme == themeType)
        {
            return;
        }

        CurrentTheme = themeType;

        var dictionaries = Application.Current.Resources.MergedDictionaries;
        var currentTheme = dictionaries.FirstOrDefault(d => _themePaths.ContainsValue(d.Source.OriginalString));

        if (currentTheme != null)
        {
            dictionaries.Remove(currentTheme);
        }

        var newThemeUri = new Uri(_themePaths[themeType], UriKind.Relative);
        var newTheme = new ResourceDictionary
        {
            Source = newThemeUri
        };

        dictionaries.Add(newTheme);
        ThemeChanged?.Invoke(themeType);
    }
}

public enum ThemeType
{
    Dark,
    Light
}