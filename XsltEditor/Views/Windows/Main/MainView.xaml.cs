using System.Runtime.Versioning;

using XsltEditor.ViewModels.Main;

namespace XsltEditor.Views.Windows.Main;

[SupportedOSPlatform("windows")]
public partial class MainView
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}