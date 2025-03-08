using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using XsltEditor.Models;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Implementation;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;
using XsltEditor.ViewModels.Base;

namespace XsltEditor.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly IMessenger _messenger;
    private readonly IThemeManager _themeManager;

    public Settings Settings { get; }
    public ObservableCollection<ThemeType> Themes { get; } = [];
    public ObservableCollection<EngineType> Engines { get; } = [];

    public SettingsViewModel(
        ISettingsService settingsService,
        IThemeManager themeManager,
        IMessenger messenger)
    {
        _themeManager = themeManager;
        _settingsService = settingsService;
        _messenger = messenger;

        InitializeCollections();

        Settings = settingsService.Settings;
    }

    public ThemeType SelectedTheme
    {
        get => Settings.Theme;
        set
        {
            Settings.Theme = value;
            SaveSettings();

            _themeManager.Apply(value);
        }
    }

    public EngineType SelectedEngine
    {
        get => Settings.Engine;
        set
        {
            Settings.Engine = value;
            SaveSettings();

            _messenger.Send(new EngineMessage(value));
        }
    }

    private void SaveSettings([CallerMemberName] string? propertyName = null)
    {
        try
        {
            _settingsService.SaveSettings();
        }
        catch (Exception ex)
        {
            LogError("Error save settings: " + propertyName, ex);
        }
    }

    private void InitializeCollections()
    {
        LoadValues(Themes);
        LoadValues(Engines);

        static void LoadValues<T>(ICollection<T> collection) where T : struct, Enum
        {
            foreach (var item in Enum.GetValues<T>())
            {
                collection.Add(item);
            }
        }
    }
}