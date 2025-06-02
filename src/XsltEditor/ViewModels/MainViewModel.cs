using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.ComponentModel;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.ViewModels;

internal partial class MainViewModel : ObservableObject
{
    private readonly IWindowService _windowService;
    private readonly ISettingsService _settingsService;

    [ObservableProperty] private string? _htmlContent;
    [ObservableProperty] private DocumentViewModel? _activeDocument;

    public MainViewModel(
        IWindowService windowService,
        ISettingsService settingsService)
    {
        _windowService = windowService;
        _settingsService = settingsService;

        Documents = 
        [
            CreateDocument("XSL", DocumentType.Xsl),
            CreateDocument("XML", DocumentType.Xml)
        ];
    }

    public ObservableCollection<DocumentViewModel> Documents { get; }

    [RelayCommand]
    private void OpenSettingsView()
    {
        _windowService.ShowDialog<SettingsViewModel>();
    }

    [RelayCommand]
    private void OpenCaretView()
    {
        _windowService.ShowDialog<CaretViewModel>();
    }

    [RelayCommand]
    private void OpenCompletionView()
    {
        _windowService.ShowDialog<CompletionViewModel>();
    }

    [RelayCommand]
    private async Task SaveDocument(DocumentType documentType)
    {
        var document = Documents.FirstOrDefault(x => x.DocumentType == documentType);

        if (document != null)
        {
            await document.SaveDocumentCommand.ExecuteAsync(null);
        }
    }

    [RelayCommand]
    private async Task OpenDocument(DocumentType documentType)
    {
        var document = Documents.FirstOrDefault(x => x.DocumentType == documentType);

        if (document != null)
        {
            await document.OpenDocumentCommand.ExecuteAsync(null);
            ActiveDocument = document;
        }
    }

    private DocumentViewModel CreateDocument(string name, DocumentType documentType)
    {
        var document = new DocumentViewModel
        {
            Name = name,
            DocumentType = documentType
        };

        AttachDocumentEvents(document);
        return document;
    }

    private void AttachDocumentEvents(DocumentViewModel document)
    {
        document.PropertyChanged += Document_PropertyChanged;
    }

    private void DetachDocumentEvents(DocumentViewModel document)
    {
        document.PropertyChanged -= Document_PropertyChanged;
    }

    private void Document_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(DocumentViewModel.Text))
        {
            return;
        }

        HtmlContent = "";
    }
}