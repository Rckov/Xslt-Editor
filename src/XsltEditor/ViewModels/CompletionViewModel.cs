using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Input;

using XsltEditor.Infrastructure;
using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels.Base;
using XsltEditor.Views.UserControls;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class CompletionViewModel : BaseViewModel
{
    private readonly ICompletionDataService _dataService;

    public CompletionViewModel(ICompletionDataService dataService)
    {
        _dataService = dataService;
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

    public ICommand? AddCommand { get; private set; }
    public ICommand? DeleteCommand { get; private set; }
    public ICommand? SaveCommand { get; private set; }

    protected override void InitializeCommands()
    {
        AddCommand = new RelayCommand(AddCompletionData);
        DeleteCommand = new RelayCommand(DeleteCompletionData, CanDeleteCompletionData);
        SaveCommand = new RelayCommand(SaveCompletionData);
    }

    private void AddCompletionData()
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

    private void DeleteCompletionData()
    {
        if (SelectedData is null)
        {
            return;
        }

        if (CompletionData.Any(x => x.Equals(SelectedData)))
        {
            CompletionData.Remove(SelectedData);
        }
    }

    private bool CanDeleteCompletionData()
    {
        return SelectedData is not null;
    }

    private void SaveCompletionData()
    {
        try
        {
            _dataService.SaveCompletionData(CompletionData);
            MessageBox.Show("Data saved successfully. Please restart the application.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            LogError("Error saving completion data", ex);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}