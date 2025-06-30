using CommunityToolkit.Mvvm.Messaging;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using System.Windows;
using System.Xml;

using XsltEditor.Models.Enums;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class ThemeService : IThemeService
{
    private readonly IMessenger _messenger;
    private readonly IResourceOperationsService _resourceService;

    private readonly Dictionary<ThemeType, ThemeInfo> _themes = [];

    public ThemeService(IMessenger messenger, IResourceOperationsService resourceService)
    {
        _messenger = messenger;
        _resourceService = resourceService;

        LoadThemes();
    }

    public ThemeType CurrentThemeType { get; private set; }

    public void SwitchTheme()
    {
        SetTheme(CurrentThemeType == ThemeType.Light
            ? ThemeType.Dark
            : ThemeType.Light);
    }

    public void SetTheme(ThemeType theme)
    {
        if (!_themes.TryGetValue(theme, out var info))
        {
            throw new InvalidOperationException($"Theme '{theme}' is not registered.");
        }

        ReplaceXamlTheme(info.XamlPath);
        var highlighting = LoadHighlighting(info.HighlightingResource);

        CurrentThemeType = theme;

        _messenger.Send(new ThemeChangedMessage(theme, highlighting));
    }

    private void ReplaceXamlTheme(string xamlPath)
    {
        var dictionaries = Application.Current.Resources.MergedDictionaries;
        var currentTheme = dictionaries.FirstOrDefault(d => _themes.Values.Any(t => t.XamlPath == d.Source?.OriginalString));

        if (currentTheme != null)
        {
            dictionaries.Remove(currentTheme);
        }

        dictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(xamlPath, UriKind.Relative)
        });
    }

    private IHighlightingDefinition? LoadHighlighting(string? resourcePath)
    {
        if (string.IsNullOrWhiteSpace(resourcePath))
        {
            return null;
        }

        using var stream = _resourceService.GetResourceStream(resourcePath);

        if (stream == null)
        {
            return null;
        }

        using var reader = XmlReader.Create(stream);
        return HighlightingLoader.Load(reader, HighlightingManager.Instance);
    }

    private void LoadThemes()
    {
        AddTheme(ThemeType.Dark,
            "Resources/Themes/DarkBrushes.xaml",
            "XsltEditor.Resources.Highlighting.DarkMode.xshd");

        AddTheme(ThemeType.Light,
            "Resources/Themes/LightBrushes.xaml",
            "XsltEditor.Resources.Highlighting.LightMode.xshd");
    }

    private void AddTheme(ThemeType type, string xamlPath, string highlightingResource)
    {
        _themes[type] = new ThemeInfo(xamlPath, highlightingResource);
    }

    private record ThemeInfo(string XamlPath, string HighlightingResource);
}