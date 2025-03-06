using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;

using XsltEditor.Helpers;
using XsltEditor.Models;
using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;

namespace XsltEditor.ViewModels.Main;

public class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        Settings = settingsService.Settings;

        InitializeCollections();
    }

    public Settings Settings { get; }
    public ObservableCollection<ThemeType> Themes { get; } = [];
    public ObservableCollection<EngineType> Engines { get; } = [];
    public ObservableCollection<int> Sizes { get; } = [1, 2, 3, 4, 5, 6, 7, 8];
    public ObservableCollection<int> FontSizes { get; } = [8, 10, 12, 14, 16, 18, 20];

    public ThemeType SelectedTheme
    {
        get => Settings.Theme;
        set
        {
            Settings.Theme = value;
            ThemeManager.Apply(value);

            SaveSettings();
        }
    }

    public int FontSize
    {
        get => Settings.FontSize;
        set
        {
            Settings.FontSize = value;
            SaveSettings();
        }
    }

    public FontFamily? FontFamily
    {
        get => new(Settings.FontFamily);
        set
        {
            Settings.FontFamily = value?.Source;
            SaveSettings();
        }
    }

    public bool ShowSpaces
    {
        get => Settings.ShowSpaces;
        set
        {
            Settings.ShowSpaces = value;
            SaveSettings();
        }
    }

    public bool ConvertTabsToSpaces
    {
        get => Settings.ConvertTabsToSpaces;
        set
        {
            Settings.ConvertTabsToSpaces = value;
            SaveSettings();
        }
    }

    public bool HighlightCurrentLine
    {
        get => Settings.HighlightCurrentLine;
        set
        {
            Settings.HighlightCurrentLine = value;
            SaveSettings();
        }
    }

    public int TabSize
    {
        get => Settings.TabSize;
        set
        {
            Settings.TabSize = value;
            SaveSettings();
        }
    }

    public int IndentSize
    {
        get => Settings.IndentSize;
        set
        {
            if (Set(ref field, value))
            {
                Settings.IndentSize = value;
                SaveSettings();
            }
        }
    }

    private void InitializeCollections()
    {
        Engines.Add(EngineType.XslCompiledTransform);

        foreach (ThemeType theme in Enum.GetValues<ThemeType>())
        {
            Themes.Add(theme);
        }
    }

    private void SaveSettings()
    {
        try
        {
            _settingsService.SaveSettings();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error save settings: {ex.Message}");
        }
    }
}