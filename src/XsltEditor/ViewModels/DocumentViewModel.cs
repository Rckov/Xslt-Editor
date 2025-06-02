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
    private readonly IFileService _fileService = App.Services.GetRequiredService<IFileService>();

    #region Document properties

    [ObservableProperty] private string? _name;
    [ObservableProperty] private string? _text;
    [ObservableProperty] private string? _filePath;
    [ObservableProperty] private bool _isDirty;
    [ObservableProperty] private bool _isReadOnly;

    #endregion Document properties

    #region Caret position

    [ObservableProperty] private int _line;
    [ObservableProperty] private int _column;

    #endregion Caret position

    #region Syntax highlighting

    [ObservableProperty] private IHighlightingDefinition? _highlighting;

    #endregion Syntax highlighting

    public DocumentType DocumentType { get; set; }

    protected override void OnActivated()
    {
        Messenger.Register<DocumentViewModel, CaretChangedMessage>(this, (r, m) => Line = m.Value);
        Messenger.Register<DocumentViewModel, ThemeChangedMessage>(this, (r, m) => Highlighting = m.Highlighting);
    }

    [RelayCommand]
    public async Task SaveDocument()
    {
        FilePath ??= _fileService.ShowSaveFileDialog($"Save {Name}", $".{DocumentType}".ToLower());

        if (string.IsNullOrWhiteSpace(FilePath))
        {
            return;
        }

        await _fileService.SaveFileContentAsync(FilePath, Text);
        IsDirty = false;
    }

    [RelayCommand]
    public async Task OpenDocument()
    {
        FilePath = _fileService.ShowOpenFileDialog("Open " + Name, $".{DocumentType}".ToLower());

        if (string.IsNullOrWhiteSpace(FilePath))
        {
            return;
        }

        Text = await _fileService.ReadFileContentAsync(FilePath);
    }

    partial void OnTextChanged(string? value) => IsDirty = true;
}

internal enum DocumentType
{
    Xsl,
    Xml
}