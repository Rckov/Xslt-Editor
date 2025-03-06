using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;

using XsltEditor.Helpers;
using XsltEditor.Models;
using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;

namespace XsltEditor.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    public Settings Settings { get; }
    public ObservableCollection<ThemeType> Themes { get; } = [];
    public ObservableCollection<EngineType> Engines { get; } = [];

    public FontFamily? FontFamily
    {
        get => new(Settings.FontFamily);
        set
        {
            Settings.FontFamily = value?.Source;
            SaveSettings();
        }
    }

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

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        Settings = settingsService.Settings;

        InitializeCollections();
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