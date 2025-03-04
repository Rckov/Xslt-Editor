using System.Windows;
using XsltEditor.ViewModels;

namespace XsltEditor.Views.Windows;

public partial class SettingsView
{
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
} 