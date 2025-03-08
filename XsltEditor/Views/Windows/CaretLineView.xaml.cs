using System.Text.RegularExpressions;
using System.Windows.Input;

using XsltEditor.ViewModels;

namespace XsltEditor.Views.Windows;

public partial class CaretLineView
{
    public CaretLineView(CaretLineViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.CloseWindow = new Action(Close);
    }

    [GeneratedRegex("[^0-9]+")]
    private static partial Regex NumberRegex();

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = NumberRegex().IsMatch(e.Text);
    }
}