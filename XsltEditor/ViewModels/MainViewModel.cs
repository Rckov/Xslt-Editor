using System.Runtime.Versioning;
using System.Windows.Input;

using XsltEditor.Core;
using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;
using XsltEditor.Tools;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class MainViewModel : ObservableObject
{
    private readonly IWindowService _windowService;

    public MainViewModel(IWindowService windowService)
    {
        _windowService = windowService;

        InitCommands();
    }

    public string? HtmlText
    {
        get;
        set => Set(ref field, value);
    }

    public ICommand? OpenFileCommand { get; private set; }
    public ICommand? SaveFileCommand { get; private set; }
    public ICommand? SwitchThemeCommand { get; private set; }
    public ICommand? OpenCompletionWindowCommand { get; private set; }

    private void InitCommands()
    {
        OpenFileCommand = new RelayCommand(OpenFile);
        SaveFileCommand = new RelayCommand(SaveFile);
        SwitchThemeCommand = new RelayCommand(SwitchTheme);
        OpenCompletionWindowCommand = new RelayCommand(OpenCompletionWindow);
    }

    private void OpenFile(object? parameter)
    {
    }

    private void SaveFile(object? parameter)
    {
    }

    private void SwitchTheme(object? obj)
    {
        var curTheme = ThemeManager.CurrentTheme;
        var newTheme = curTheme == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark;

        ThemeManager.Apply(newTheme);
    }

    private void OpenCompletionWindow(object? parameter)
    {
    }
}