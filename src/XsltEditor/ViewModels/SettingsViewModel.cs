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

        InitializeSettings();
        InitializeCollections();
    }

    public Settings? Settings { get; private set; }
    public ObservableCollection<ThemeType> Themes { get; } = [];
    public ObservableCollection<EngineType> Engines { get; } = [];

    [RelayCommand]
    private void SaveSettings()
    {
        try
        {
            if (Settings != null)
            {
                _settingsService.SaveSettings();
            }
            else
            {
                _windowService.ShowMessage("Settings are not initialized.", "Error");
            }
        }
        catch (Exception ex)
        {
            _windowService.ShowMessage($"Failed to save settings: {ex.Message}", "Error");
        }
    }

    private void InitializeSettings()
    {
        Settings = _settingsService.Settings;
    }

    private void InitializeCollections()
    {
        Themes.LoadFromEnum();
        Engines.LoadFromEnum();
    }

    partial void OnSelectedThemeChanged(ThemeType value)
    {
        UpdateSetting(
            s => s.Theme = value, 
            () => _themeService.ChangeTheme(value));
    }

    partial void OnSelectedEngineChanged(EngineType value)
    {
        UpdateSetting(
            s => s.Engine = value, 
            () => _transformService.CreateEngine(value));
    }

    private void UpdateSetting(Action<Settings> updateAction, Action? action = null)
    {
        try
        {
            action?.Invoke();

            if (Settings != null && updateAction != null)
            {
                updateAction(Settings);
            }
        }
        catch (Exception ex)
        {
            _windowService.ShowMessage($"Error applying setting: {ex.Message}", "Error");
        }
    }
}