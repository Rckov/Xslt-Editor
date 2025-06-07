using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.ViewModels;

internal partial class CompletionViewModel(IWindowService windowService, ICompletionDataService completionService) : ObservableObject
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private CompletionData? _selectedData;

    public ObservableCollection<CompletionData> Items { get; } = new(completionService.Data);

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            await completionService.SaveCompletionData();
            windowService.ShowMessage("Data saved successfully.", "Success");
        }
        catch (Exception ex)
        {
            windowService.ShowMessage("Error saving " + ex.Message, "Error");
        }
    }

    [RelayCommand]
    private void Add()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        if (Items.Any(x => string.Equals(x.Text, Name, StringComparison.OrdinalIgnoreCase)))
        {
            windowService.ShowMessage("Item already exists.", "Duplicate Item");
            return;
        }

        var newItem = new CompletionData(Name);
        completionService.Add(newItem);
        Items.Add(newItem);

        Name = null;
    }

    [RelayCommand]
    private void Delete()
    {
        if (SelectedData is null)
        {
            return;
        }

        if (!Items.Contains(SelectedData))
        {
            return;
        }

        completionService.Remove(SelectedData);
        Items.Remove(SelectedData);

        SelectedData = null;
    }
}