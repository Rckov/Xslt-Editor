using System.Runtime.Versioning;

using XsltEditor.ViewModels;

namespace XsltEditor.Views.Windows;

[SupportedOSPlatform("windows")]
public partial class MainView
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}