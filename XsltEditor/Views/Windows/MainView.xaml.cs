using Microsoft.Web.WebView2.Core;

using System.ComponentModel;
using System.Runtime.Versioning;

using XsltEditor.ViewModels;

namespace XsltEditor.Views.Windows;

[SupportedOSPlatform("windows")]
public partial class MainView
{
    public MainView(MainViewModel viewModel)
    {
        DataContext = viewModel;

        InitializeComponent();
        InitializeWebView();
    }

    private async void InitializeWebView()
    {
        var webView2Environment = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions
        {
            AreBrowserExtensionsEnabled = false
        });

        await WebView.EnsureCoreWebView2Async(webView2Environment);
        //WebView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;

        if (WebView.DataContext is MainViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
    }

    private async void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        await WebView.CoreWebView2.ExecuteScriptAsync(@"
            (function() {
                document.querySelectorAll('body *').forEach(el => {
                    el.style.color = 'white';
                    el.style.backgroundColor = 'transparent';

                    // Обрабатываем границы таблиц
                    if (el.tagName === 'TABLE' || el.tagName === 'TD' || el.tagName === 'TH') {
                        el.style.border = '1px solid white';
                    }
                });

                document.querySelectorAll('table').forEach(table => {
                    table.style.color = 'white';
                    table.style.borderColor = 'white';
                    table.style.backgroundColor = 'transparent';
                });
            })();
        ");
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.HtmlContent) && sender is MainViewModel viewModel)
        {
            WebView.NavigateToString(viewModel.HtmlContent);
        }
    }
}