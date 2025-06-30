using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Windows;

using XsltEditor.Common.Attributes;
using XsltEditor.Models;
using XsltEditor.Services.Interfaces;
using XsltEditor.Views.Dialogs;

namespace XsltEditor.ViewModels;

[Window(typeof(CompletionDialog))]
internal partial class CompletionViewModel(ICompletionDataService completionService) : ObservableObject
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private CompletionData? _selectedData;

    public ObservableCollection<CompletionData> CompletionData { get; } = new(completionService.Data);

    [RelayCommand]
    private void Save()
    {
        try
        {
            completionService.SaveData();
            MessageBox.Show("Data saved successfully.", "Success");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error saving " + ex.Message, "Error");
        }
    }

    [RelayCommand]
    private void Add()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        if (CompletionData.Any(x => string.Equals(x.Text, Name, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Item already exists.", "Duplicate Item");
            return;
        }

        var newItem = new CompletionData(Name);
        completionService.Add(newItem);
        CompletionData.Add(newItem);

        Name = null;
    }

    [RelayCommand]
    private void Delete()
    {
        if (SelectedData is null)
        {
            return;
        }

        if (!CompletionData.Contains(SelectedData))
        {
            return;
        }

        completionService.Remove(SelectedData);
        CompletionData.Remove(SelectedData);

        SelectedData = null;
    }
}