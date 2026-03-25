using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using ICSharpCode.AvalonEdit.Highlighting;

using System.Diagnostics;
using System.IO;
using System.Text;

using XsltEditor.Models;
using XsltEditor.Models.Messages;
using XsltEditor.Sdk.Enums;
using XsltEditor.Services.Abstractions;

namespace XsltEditor.ViewModels;

internal partial class DocumentViewModel : ObservableObject
{
	private readonly IWindowService _windowService;
	private readonly IFileDialogService _fileDialog;

	[ObservableProperty] private string? _content;
	[ObservableProperty] private string? _filePath;
	[ObservableProperty] private IHighlightingDefinition? _highlighting;
	[ObservableProperty] private int _line = 1;
	[ObservableProperty] private int _column = 1;
	[ObservableProperty] private bool _isDirty;
	[ObservableProperty] private bool _isReadOnly;
	[ObservableProperty] private Encoding _encoding = Encoding.UTF8;

	public string EditMode => IsReadOnly ? "Read Only" : "Write Mode";
	public string EncodingName => Encoding.WebName.ToUpperInvariant();

	public string Name { get; set; } = string.Empty;
	public DocumentType DocumentType { get; set; }
	public IReadOnlyList<SnippetData>? CompletionData { get; set; }

	public DocumentViewModel(
		IMessenger messenger,
		IWindowService windowService,
		IFileDialogService fileDialog)
	{
		_windowService = windowService;
		_fileDialog = fileDialog;

		messenger.Register<ThemeChangedMessage>(this, (_, m) => Highlighting = m.Highlighting);
	}

	[RelayCommand]
	private void OpenExplorer()
	{
		if (FilePath is not { } path)
		{
			return;
		}

		Process.Start("explorer.exe", $"/select,\"{path}\"");
	}

	[RelayCommand]
	private void GoToLine()
	{
		GoToLineViewModel? vm = _windowService.ShowWindow(new GoToLineViewModel(Line), dialog: true);

		if (vm?.Result is { } line)
		{
			Line = line;
		}
	}

	[RelayCommand]
	private async Task SaveDocument()
	{
		FilePath ??= _fileDialog.OpenSaveDialog($"Save {Name}", $".{DocumentType}".ToLower());

		if (string.IsNullOrWhiteSpace(FilePath))
		{
			return;
		}

		await File.WriteAllTextAsync(FilePath, Content, Encoding);
		IsDirty = false;
	}

	[RelayCommand]
	private async Task OpenDocument()
	{
		var pathFile = _fileDialog.OpenFileDialog("Open " + Name, $".{DocumentType}".ToLower());

		if (string.IsNullOrWhiteSpace(pathFile))
		{
			return;
		}

		using StreamReader reader = new(pathFile, detectEncodingFromByteOrderMarks: true);
		Content = await reader.ReadToEndAsync();
		Encoding = reader.CurrentEncoding;

		IsDirty = true;
		FilePath = pathFile;
	}

	partial void OnContentChanged(string? value)
	{
		IsDirty = true;
	}

	partial void OnIsReadOnlyChanged(bool value)
	{
		OnPropertyChanged(nameof(EditMode));
	}

	partial void OnEncodingChanged(Encoding value)
	{
		OnPropertyChanged(nameof(EncodingName));
	}
}
