using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using XsltEditor.Extensions;
using XsltEditor.Models;
using XsltEditor.Services;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;

namespace XsltEditor.ViewModels;

internal partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeService _themeService;
    private readonly ISettingsService _settingsService;
    private readonly IXmlTransformService _transformService;
    private readonly IWindowService _windowService;

    [ObservableProperty] private ThemeType _selectedTheme;
    [ObservableProperty] private EngineType _selectedEngine;

    private readonly Settings _settings;

    public SettingsViewModel(
        IThemeService themeService,
        ISettingsService settingsService,
        IXmlTransformService transformService,
        IWindowService windowService)
    {
        _themeService = themeService;
        _settingsService = settingsService;
        _transformService = transformService;
        _windowService = windowService;

        _settings = _settingsService.Settings;

        SelectedTheme = _settings.Theme;
        SelectedEngine = _settings.Engine;

        InitializeCollections();
    }

    public event Action<bool>? CloseRequest;

    public ObservableCollection<ThemeType> Themes { get; } = [];
    public ObservableCollection<EngineType> Engines { get; } = [];

    private void InitializeCollections()
    {
        Themes.LoadFromEnum();
        Engines.LoadFromEnum();
    }

    [RelayCommand]
    private async Task SaveSettings()
    {
        try
        {
            ApplyTheme();
            ApplyEngine();

            await _settingsService.SaveSettings();
        }
        catch (Exception ex)
        {
            _windowService.ShowMessage($"Failed to save settings: {ex.Message}", "Error");
        }

        CloseRequest?.Invoke(true);
    }

    private void ApplyTheme()
    {
        _settings.Theme = SelectedTheme;
        _themeService.ChangeTheme(SelectedTheme);
    }

    private void ApplyEngine()
    {
        _settings.Engine = SelectedEngine;
        _transformService.CreateEngine(SelectedEngine);
    }
}