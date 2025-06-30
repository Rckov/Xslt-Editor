using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using XsltEditor.ViewModels;

namespace XsltEditor.Views.Dialogs;

public partial class CaretDialog
{
    public CaretDialog()
    {
        InitializeComponent();
        Loaded += OnWindowLoaded;
    }

    [GeneratedRegex("[^0-9]+")]
    private static partial Regex NumberRegex();

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = NumberRegex().IsMatch(e.Text);
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is CaretViewModel viewModel)
        {
            viewModel.CloseRequest += OnCloseRequest;
        }
    }

    private void OnCloseRequest(bool dialogResult)
    {
        DialogResult = dialogResult;
        Close();
    }
}