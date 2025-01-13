using System.Windows;

using XsltEditor.ViewModels;

namespace XsltEditor.Views;

public partial class SettingsView : Window
{
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}