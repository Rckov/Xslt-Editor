using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Input;

using XsltEditor.Models;
using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;
using XsltEditor.Tools.Commands;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class CompletionViewModel : ObservableObject
{
    private readonly ICompletionDataService _dataService;

    public CompletionViewModel(ICompletionDataService dataService)
    {
        _dataService = dataService;

        AddCommand = new RelayCommand(AddCompletionData);
        DeleteCommand = new RelayCommand(DeleteCompletionData, CanDeleteCompletionData);
        SaveCommand = new RelayCommand(SaveCompletionData);

        CompletionData = new ObservableCollection<CompletionData>(dataService.LoadCompletionData());
    }

    public string? NameData
    {
        get;
        set => Set(ref field, value);
    }

    public CompletionData? SelectedData
    {
        get;
        set => Set(ref field, value);
    }

    public ObservableCollection<CompletionData> CompletionData { get; }

    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SaveCommand { get; }

    private void AddCompletionData(object? parameter)
    {
        if (string.IsNullOrEmpty(NameData))
        {
            return;
        }

        if (CompletionData.Any(x => x.Text == NameData))
        {
            MessageBox.Show("Item already exists.", "Duplicate Item", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        CompletionData.Add(new CompletionData(NameData));
        NameData = null;
    }

    private void DeleteCompletionData(object? parameter)
    {
        if (SelectedData is null)
        {
            return;
        }

        if (CompletionData.Any(x => x.Equals(SelectedData))) CompletionData.Remove(SelectedData);
    }

    private bool CanDeleteCompletionData(object? parameter)
    {
        return SelectedData is not null;
    }

    private void SaveCompletionData(object? parameter)
    {
        try
        {
            _dataService.SaveCompletionData(CompletionData);
            MessageBox.Show("Data saved successfully. Please restart the application.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}