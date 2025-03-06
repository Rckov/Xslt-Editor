using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

using XsltEditor.Helpers;
using XsltEditor.Infrastructure;
using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;
using XsltEditor.Views.Windows;
using XsltEditor.Views.Windows.Dialogs;

namespace XsltEditor.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly IWindowService _windowService;
    private readonly ISettingsService _settingsService;

    public ObservableCollection<DocumentViewModel> Documents { get; set; }

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

    public MainViewModel(IWindowService windowService, ISettingsService settingsService)
    {
        _windowService = windowService;
        _settingsService = settingsService;

        Documents = [
            new DocumentViewModel("XSL", settingsService),
            new DocumentViewModel("XML", settingsService) { IsReadOnly = true }
        ];

        InitCommands();

        ThemeManager.Apply(_settingsService.Settings.Theme);
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

        var path = ActiveDocument.FilePath;

        if (string.IsNullOrEmpty(path))
        {
            path = Dialog.SaveFile("Save File", ".xsl", ".xslt", ".xml");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }
        }

        await ActiveDocument.SaveDocument(path);
    }

    private void OpenFileInExplorer(object? parameter)
    {
        if (ActiveDocument?.FilePath is not null)
        {
            Process.Start("explorer.exe", $"/select,\"{ActiveDocument.FilePath}\"");
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