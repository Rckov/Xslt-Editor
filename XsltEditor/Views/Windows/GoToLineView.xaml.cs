using System.Text.RegularExpressions;

using XsltEditor.ViewModels;

namespace XsltEditor.Views.Windows;

public partial class GoToLineView
{
    public GoToLineView(GoToLineViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.CloseWindow = new Action(Close);
    }

    [GeneratedRegex("[^0-9]+")]
    private static partial Regex NumberRegex();

    private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = NumberRegex().IsMatch(e.Text);
    }
}