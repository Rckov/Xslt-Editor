using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace XsltEditor.Views;

public partial class GoToLineWindow
{
    public GoToLineWindow()
    {
        InitializeComponent();
    }

    private void OnGoClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = NonDigitRegex().IsMatch(e.Text);
    }

    [GeneratedRegex("[^0-9]+")]
    private static partial Regex NonDigitRegex();
}