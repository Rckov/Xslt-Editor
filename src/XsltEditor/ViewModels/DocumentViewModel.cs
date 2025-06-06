using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using ICSharpCode.AvalonEdit.Highlighting;

using Microsoft.Extensions.DependencyInjection;

using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.ViewModels;

internal partial class DocumentViewModel : ObservableRecipient
{
    private readonly IDialogService _dialogService = App.Services.GetRequiredService<IDialogService>();
    private readonly IFileOperationsService _fileService = App.Services.GetRequiredService<IFileOperationsService>();

    [ObservableProperty] private string? _name;
    [ObservableProperty] private string? _text;
    [ObservableProperty] private string? _filePath;
    [ObservableProperty] private bool _isDirty;
    [ObservableProperty] private bool _isReadOnly;

    [ObservableProperty] private int _line;
    [ObservableProperty] private int _column;

    [ObservableProperty] private IHighlightingDefinition? _highlighting;

    public DocumentType DocumentType { get; init; }

    protected override void OnActivated()
    {
        Messenger.Register<DocumentViewModel, CaretChangedMessage>(this, (_, m) => Line = m.Value);
        Messenger.Register<DocumentViewModel, ThemeChangedMessage>(this, (_, m) => Highlighting = m.Highlighting);
    }

    [RelayCommand]
    private async Task SaveDocument()
    {
        FilePath ??= _dialogService.ShowSaveFileDialog($"Save {Name}", $".{DocumentType}".ToLower());

        if (string.IsNullOrWhiteSpace(FilePath))
        {
            return;
        }

        await _fileService.SaveAsync(FilePath, Text);
        IsDirty = false;
    }

    [RelayCommand]
    private async Task OpenDocument()
    {
        FilePath = _dialogService.ShowOpenFileDialog("Open " + Name, $".{DocumentType}".ToLower());

        if (string.IsNullOrWhiteSpace(FilePath))
        {
            return;
        }

        Text = await _fileService.ReadAsync(FilePath);
    }

    partial void OnTextChanged(string? value) => IsDirty = true;
}

internal enum DocumentType
{
    Xsl,
    Xml
}