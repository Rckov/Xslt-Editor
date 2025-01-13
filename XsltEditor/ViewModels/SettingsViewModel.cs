using System.Runtime.Versioning;

using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }
}