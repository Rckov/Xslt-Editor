using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using ICSharpCode.AvalonEdit.Highlighting;

using System.Diagnostics;

using XsltEditor.Models;
using XsltEditor.Models.Enums;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.ViewModels;

internal partial class DocumentViewModel : ObservableObject
{
    private readonly IFileDialogService _fileService;
    private readonly IDocumentStorageService _storageService;
    private readonly IWindowService _windowService;

    [ObservableProperty] private int _line;
    [ObservableProperty] private int _column;

    [ObservableProperty] private string? _content;
    [ObservableProperty] private string? _filePath;

    [ObservableProperty] private bool _isDirty;
    [ObservableProperty] private bool _isReadOnly;

    [ObservableProperty] private IHighlightingDefinition? _highlighting;

    public DocumentViewModel(
        IMessenger messenger,
        IFileDialogService fileService,
        IDocumentStorageService storageService,
        IWindowService windowService,
        ICompletionDataService completionData)
    {
        _fileService = fileService;
        _storageService = storageService;
        _windowService = windowService;

        completionData.LoadData(DocumentType);
        CompletionData = completionData.Data;

        messenger.Register<CaretChangedMessage>(this, (_, m) =>
        {
            if (m.Id == Id)
            {
                Line = m.Value;
            }
        });

        messenger.Register<ThemeChangedMessage>(this, (_, m) => Highlighting = m.Highlighting);
    }

    public Guid Id { get; init; }
    public string? Name { get; init; }
    public DocumentType DocumentType { get; init; }
    public IReadOnlyList<CompletionData> CompletionData { get; } = [];

    [RelayCommand]
    private void OpenCaretView()
    {
        _windowService.ShowDialog<CaretViewModel>(Id);
    }

    [RelayCommand]
    private void OpenExplorer()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
        {
            return;
        }

        Process.Start("explorer.exe", $"/select,\"{FilePath}\"");
    }

    [RelayCommand]
    private async Task OpenDocument()
    {
        var pathFile = _fileService.OpenFileDialog("Open " + Name, $".{DocumentType}".ToLower());

        if (string.IsNullOrWhiteSpace(pathFile))
        {
            return;
        }

        Content = await _storageService.ReadContentAsync(pathFile);

        IsDirty = true;
        FilePath = pathFile;
    }

    [RelayCommand]
    private async Task SaveDocument()
    {
        FilePath ??= _fileService.OpenSaveDialog($"Save {Name}", $".{DocumentType}".ToLower());

        if (string.IsNullOrWhiteSpace(FilePath))
        {
            return;
        }

        await _storageService.WriteContentAsync(FilePath, Content);
        IsDirty = false;
    }

    partial void OnContentChanged(string? value) => IsDirty = true;
}