using Microsoft.Web.WebView2.Core;

using System.ComponentModel;

using Wpf.Ui.Controls;

using XsltEditor.ViewModels;

namespace XsltEditor.Views;

public partial class MainView : FluentWindow
{
    public MainView()
    {
        InitializeComponent();
        InitializeWebViewBrowser();
    }

    private async void InitializeWebViewBrowser()
    {
        await WebView.EnsureCoreWebView2Async(null);
        WebView.NavigationCompleted += WebView_NavigationCompleted;

        if (DataContext is MainViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.HtmlText))
        {
            if (sender is MainViewModel viewModel)
            {
                WebView.NavigateToString(viewModel.HtmlText);
            }
        }
    }

    private async void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        await SetDefaultTextColorAsync();
    }

    private async Task SetDefaultTextColorAsync()
    {
        await WebView.ExecuteScriptAsync("document.body.style.color = 'white';");
    }
}