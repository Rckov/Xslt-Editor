using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using ICSharpCode.AvalonEdit.Highlighting;

using System.ComponentModel.DataAnnotations;

using XsltEditor.Models;
using XsltEditor.Models.Enums;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.ViewModels;

internal partial class DocumentViewModel : ObservableObject
{
    private readonly IFileDialogService _fileService;
    private readonly IDocumentStorageService _storageService;

    [ObservableProperty] private string? _content;
    [ObservableProperty] private string? _filePath;

    [ObservableProperty] private int _line;
    [ObservableProperty] private int _column;

    [ObservableProperty] private bool _isDirty;
    [ObservableProperty] private bool _isReadOnly;

    [ObservableProperty] private IHighlightingDefinition? _highlighting;

    public DocumentViewModel(
        IMessenger messenger,
        IFileDialogService fileService,
        IDocumentStorageService storageService)
    {
        _fileService = fileService;
        _storageService = storageService;
    }

    [Required]
    public Guid Id { get; init; }

    [Required]
    public string? Name { get; init; }

    [Required]
    public DocumentType DocumentType { get; init; }

    public IReadOnlyList<CompletionData> CompletionData { get; } = [];

    [RelayCommand]
    private async Task OpenDocument()
    {
        FilePath = _fileService.OpenFileDialog("Open " + Name, $".{DocumentType}".ToLower());

        if (string.IsNullOrWhiteSpace(FilePath))
        {
            return;
        }

        IsDirty = false;
        Content = await _storageService.ReadContentAsync(FilePath);
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
    }

    partial void OnContentChanged(string? value) => IsDirty = true;
}