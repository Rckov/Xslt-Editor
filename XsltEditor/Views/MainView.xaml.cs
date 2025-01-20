
using Microsoft.Web.WebView2.Core;

using System.ComponentModel;
using System.Runtime.Versioning;

using XsltEditor.ViewModels;

namespace XsltEditor.Views;

[SupportedOSPlatform("windows")]
public partial class MainView
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}