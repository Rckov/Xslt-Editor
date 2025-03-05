using System.Collections.ObjectModel;
using System.Windows.Input;

using XsltEditor.Helpers;
using XsltEditor.Infrastructure;
using XsltEditor.Models.Base;
using XsltEditor.Services;
using XsltEditor.Services.Interfaces;
using XsltEditor.Views.Windows.Dialogs;
using XsltEditor.Views.Windows.Main;

namespace XsltEditor.ViewModels.Main;

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

    public ICommand? OpenFileCommand { get; private set; }
    public ICommand? SaveCommand { get; private set; }
    public ICommand? OpenCompletionWindowCommand { get; private set; }
    public ICommand? OpenGoToLineWindowCommand { get; private set; }
    public ICommand? OpenSettingsWindowCommand { get; private set; }
    public ICommand? OpenFileInExplorerCommand { get; private set; }

    public MainViewModel(IWindowService windowService)
    {
        _windowService = windowService;

        InitCommands();

        ThemeManager.Apply(ThemeType.Dark);
    }

    private void InitCommands()
    {
        OpenFileCommand = new RelayCommand(OpenFile);
        SaveCommand = new RelayCommand(SaveFile);
        OpenCompletionWindowCommand = new RelayCommand(OpenCompletionWindow);
        OpenGoToLineWindowCommand = new RelayCommand(OpenGoToLineWindow);
        OpenSettingsWindowCommand = new RelayCommand(OpenSettingWindow);
        OpenFileInExplorerCommand = new RelayCommand(OpenFileInExplorer);
    }

    private async void OpenFile(object? parameter)
    {
        var filePath = Dialog.OpenFile("Open File", ".xsl", ".xslt", ".xml");
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        var document = GetDocumentByExtension(filePath);
        if (document is not null)
        {
            await document.OpenDocument(filePath);
            ActiveDocument = document;
        }
    }

    private async void SaveFile(object? parameter)
    {
        if (ActiveDocument is null)
        {
            return;
        }

        var filePath = string.IsNullOrEmpty(ActiveDocument.FilePath)
            ? Dialog.SaveFile("Save File", ".xsl", ".xslt", ".xml")
            : ActiveDocument.FilePath;

        if (!string.IsNullOrEmpty(filePath))
        {
            await ActiveDocument.SaveDocument(filePath);
        }
    }

    private void OpenFileInExplorer(object? parameter)
    {
        if (ActiveDocument?.FilePath is not null)
        {
            System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{ActiveDocument.FilePath}\"");
        }
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

    private DocumentViewModel? GetDocumentByExtension(string filePath)
    {
        var extension = System.IO.Path.GetExtension(filePath).ToLower();
        return extension switch
        {
            ".xsl" or ".xslt" => Documents.FirstOrDefault(d => d.Name == "XSL"),
            ".xml" => Documents.FirstOrDefault(d => d.Name == "XML"),
            _ => null
        };
    }
}