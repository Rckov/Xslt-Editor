using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using System.Reflection;
using System.Windows;
using System.Xml;

namespace XsltEditor.Helpers;

internal class ThemeManager
{
    private static readonly Dictionary<ThemeType, string> _themePaths = new()
    {
        { ThemeType.Dark, "Resources/Themes/DarkBrushes.xaml" },
        { ThemeType.Light, "Resources/Themes/LightBrushes.xaml" }
    };

    private static readonly Dictionary<ThemeType, string> _highlightingPaths = new()
    {
        { ThemeType.Dark, "XsltEditor.Resources.Highlighting.DarkMode.xshd" },
        { ThemeType.Light, "XsltEditor.Resources.Highlighting.LightMode.xshd" }
    };

    public static ThemeType CurrentTheme { get; private set; }
    public static IHighlightingDefinition? CurrentHighlighting { get; private set; }

    public static event Action<ThemeType>? ThemeChanged;

    public static void Apply(ThemeType themeType)
    {
        /* Needs refactoring */
        ApplyTheme(themeType);
        ApplyHighlighting(themeType);

        CurrentTheme = themeType;
        ThemeChanged?.Invoke(themeType);
    }

    private static void ApplyTheme(ThemeType themeType)
    {
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
    }

    private static void ApplyHighlighting(ThemeType themeType)
    {
        var highlighting = _highlightingPaths[themeType];
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(highlighting);

        if (stream == null)
        {
            return;
        }

        using var reader = XmlReader.Create(stream);
        CurrentHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
    }
}

public enum ThemeType
{
    Dark,
    Light
}