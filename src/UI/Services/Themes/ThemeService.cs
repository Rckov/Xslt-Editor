using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using CommunityToolkit.Mvvm.Messaging;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using XsltEditor.Models;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Abstractions.Themes;

namespace XsltEditor.Services.Themes;

public class ThemeService(IThemeProvider provider, IMessenger messenger) : IThemeService
{
    private ResourceDictionary? _currentBrushes;

    public ThemeType CurrentTheme { get; private set; }
    public IHighlightingDefinition? Highlighting { get; private set; }

    public void SetTheme(ThemeType theme)
    {
        var descriptor = provider.Get(theme);

        ApplyBrushes(descriptor.BrushesUri);
        Highlighting = LoadHighlighting(descriptor.HighlightingUri);
        CurrentTheme = theme;

        messenger.Send(new ThemeChangedMessage(theme, Highlighting));
    }

    public void SwitchTheme()
    {
        var next = CurrentTheme is ThemeType.Default
            ? ThemeType.Dark
            : ThemeType.Default;

        SetTheme(next);
    }

    private void ApplyBrushes(string uri)
    {
        Collection<ResourceDictionary> merged = Application.Current.Resources.MergedDictionaries;

        if (_currentBrushes is not null)
        {
            merged.Remove(_currentBrushes);
        }

        _currentBrushes = new ResourceDictionary
        {
            Source = new Uri(uri, UriKind.Absolute)
        };

        merged.Add(_currentBrushes);
    }

    private static IHighlightingDefinition? LoadHighlighting(string uri)
    {
        var info = Application.GetResourceStream(new Uri(uri, UriKind.Absolute));

        if (info is null)
        {
            return null;
        }

        using var reader = XmlReader.Create(info.Stream);
        return HighlightingLoader.Load(reader, HighlightingManager.Instance);
    }
}