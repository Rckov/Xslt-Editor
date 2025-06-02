using CommunityToolkit.Mvvm.Messaging;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using System.Reflection;
using System.Windows;
using System.Xml;

using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class ThemeService : IThemeService
{
    private readonly IMessenger _messenger;
    private readonly Dictionary<ThemeType, ThemeInfo> _themes = [];

    public ThemeService(IMessenger messenger)
    {
        _messenger = messenger;

        AddTheme(
            ThemeType.Dark,
            "Resources/Themes/DarkBrushes.xaml",
            "XsltEditor.Resources.Highlighting.DarkMode.xshd");

        AddTheme(
            ThemeType.Light,
            "Resources/Themes/LightBrushes.xaml",
            "XsltEditor.Resources.Highlighting.LightMode.xshd");
    }

    public ThemeType CurrentTheme { get; private set; }

    public void ChangeTheme(ThemeType themeType)
    {
        if (!_themes.TryGetValue(themeType, out var info))
        {
            throw new InvalidOperationException($"Theme '{themeType}' is not registered.");
        }

        var dictionaries = Application.Current.Resources.MergedDictionaries;
        var currentTheme = dictionaries.FirstOrDefault(d => _themes.Values.Any(t => t.XamlPath == d.Source.OriginalString));

        if (currentTheme != null)
        {
            dictionaries.Remove(currentTheme);
        }

        CurrentTheme = themeType;

        dictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(info.XamlPath, UriKind.Relative)
        });

        _messenger.Send(new ThemeChangedMessage(themeType, LoadHighlightingDefinition(info.HighlightingPath)));
    }

    private IHighlightingDefinition? LoadHighlightingDefinition(string? resourcePath)
    {
        if (string.IsNullOrWhiteSpace(resourcePath))
        {
            return null;
        }

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
        if (stream is null)
        {
            return null;
        }

        using var reader = XmlReader.Create(stream);
        return HighlightingLoader.Load(reader, HighlightingManager.Instance);
    }

    private void AddTheme(ThemeType themeType, string xamlPath, string highlightingPath)
    {
        _themes[themeType] = new ThemeInfo(xamlPath, highlightingPath);
    }

    private sealed record ThemeInfo(string XamlPath, string HighlightingPath);
}

public enum ThemeType
{
    Dark,
    Light
}