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

    public MainViewModel(IWindowService windowService)
    {
        _windowService = windowService;

        Documents.Add(new DocumentViewModel("XSL"));
        Documents.Add(new DocumentViewModel("XML") { IsReadOnly = true });

        InitCommands();

        ThemeManager.Apply(ThemeType.Dark);
    }

    public ObservableCollection<DocumentViewModel> Documents { get; set; } = [];

    public ICommand? OpenFileCommand { get; private set; }
    public ICommand? OpenCompletionWindowCommand { get; private set; }
    public ICommand? OpenGoToLineWindowCommand { get; private set; }

    public DocumentViewModel? ActiveDocument
    {
        get;
        set => Set(ref field, value);
    }

    private void InitCommands()
    {
        OpenFileCommand = new RelayCommand(OpenFile);
        OpenCompletionWindowCommand = new RelayCommand(OpenCompletionWindow);
        OpenGoToLineWindowCommand = new RelayCommand(OpenGoToLineWindow);
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
}