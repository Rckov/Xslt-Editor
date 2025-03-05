using XsltEditor.ViewModels.Main;

namespace XsltEditor.Views.Windows.Main;

public partial class SettingsView
{
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}