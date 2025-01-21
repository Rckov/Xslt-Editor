using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Xaml.Behaviors;

using System.ComponentModel;
using System.Runtime.Versioning;
using System.Windows;

using XsltEditor.ViewModels;

namespace XsltEditor.Core.Behaviors;

[SupportedOSPlatform("windows")]
internal class WebView2Behavior : Behavior<WebView2>
{
    protected override async void OnAttached()
    {
        base.OnAttached();

        try
        {
            await InitializeWebViewBrowserAsync();
        }
        catch
        {
            MessageBox.Show(
                "An error occurred while loading the web component. Please restart the application and try again.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject.DataContext is MainViewModel viewModel)
        {
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        base.OnDetaching();
    }

    private async Task InitializeWebViewBrowserAsync()
    {
        var webView2Environment = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions
        {
            AreBrowserExtensionsEnabled = false
        });

        await AssociatedObject.EnsureCoreWebView2Async(webView2Environment);

        if (AssociatedObject.DataContext is MainViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.HtmlText) && sender is MainViewModel viewModel)
        {
            AssociatedObject.NavigateToString(viewModel.HtmlText);
        }
    }
}