using System.Collections.ObjectModel;
using System.Windows.Input;

using XsltEditor.Infrastructure;
using XsltEditor.Models.Base;
using XsltEditor.Services;
using XsltEditor.Services.Interfaces;
using XsltEditor.Views.Windows;

namespace XsltEditor.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly IWindowService _windowService;

    public ObservableCollection<DocumentViewModel> Documents { get; set; } = [
        new("XSL"),
        new("XML") { IsReadOnly = true }
    ];

    public DocumentViewModel? ActiveDocument
    {
        get;
        set => Set(ref field, value);
    }

    public ICommand OpenFileCommand { get; private set; } = null!;
    public ICommand OpenCompletionWindowCommand { get; private set; } = null!;
    public ICommand OpenGoToLineWindowCommand { get; private set; } = null!;
    public ICommand OpenSettingsWindowCommand { get; private set; } = null!;

    public MainViewModel(IWindowService windowService)
    {
        _windowService = windowService;

        InitCommands();

        ThemeManager.Apply(ThemeType.Dark);
    }

    private void InitCommands()
    {
        OpenFileCommand = new RelayCommand(OpenFile);
        OpenCompletionWindowCommand = new RelayCommand(OpenCompletionWindow);
        OpenGoToLineWindowCommand = new RelayCommand(OpenGoToLineWindow);
        OpenSettingsWindowCommand = new RelayCommand(OpenSettingWindow);
    }

    private void OpenFile(object? parameter)
    {
        throw new NotImplementedException();
    }

    private void OpenCompletionWindow(object? parameter)
    {
        _windowService.ShowDialogWindow<CompletionView>();
    }

    private void OpenGoToLineWindow(object? parameter)
    {
        _windowService.ShowDialogWindow<GoToLineView>();
    }

    private void OpenSettingWindow(object? parameter)
    {
        _windowService.ShowDialogWindow<SettingsView>();
    }
}