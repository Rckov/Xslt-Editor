using System.Runtime.Versioning;
using System.Windows.Input;

using XsltEditor.Models;
using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;
using XsltEditor.Tools.Commands;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;

        Settings = settingsService.LoadSettings();
        SaveCommand = new RelayCommand(SaveSettings);
    }

    public Settings? Settings { get; }
    public ICommand SaveCommand { get; }

    private void SaveSettings(object? parameters)
    {
        if (Settings is null) 
        {
            return;
        }

        _settingsService.SaveSettings(Settings);
    }
}