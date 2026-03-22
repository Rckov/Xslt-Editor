using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using XsltEditor.Common.Attributes;
using XsltEditor.Models;
using XsltEditor.Services.Abstractions;

namespace XsltEditor.ViewModels;

[Window(typeof(Views.SnippetWindow))]
internal partial class SnippetViewModel(ISnippetService snippetService) : ObservableObject
{
	[ObservableProperty] private string _newTag = string.Empty;
	[ObservableProperty] private SnippetData? _selectedItem;

	public ObservableCollection<SnippetData> Items { get; } = new(snippetService.Data);

	[RelayCommand(CanExecute = nameof(CanAdd))]
	private void Add()
	{
		var item = new SnippetData(NewTag.Trim());
		snippetService.Add(item);
		Items.Add(item);
		NewTag = string.Empty;
	}

	private bool CanAdd()
	{
		return !string.IsNullOrWhiteSpace(NewTag);
	}

	[RelayCommand(CanExecute = nameof(CanRemove))]
	private void Remove()
	{
		snippetService.Remove(SelectedItem!);
		Items.Remove(SelectedItem!);
		SelectedItem = null;
	}

	private bool CanRemove()
	{
		return SelectedItem is not null;
	}

	[RelayCommand]
	private void Save()
	{
		snippetService.Save();
	}

	partial void OnNewTagChanged(string value)
	{
		AddCommand.NotifyCanExecuteChanged();
	}

	partial void OnSelectedItemChanged(SnippetData? value)
	{
		RemoveCommand.NotifyCanExecuteChanged();
	}
}