using Microsoft.Web.WebView2.Core;

using System.ComponentModel;
using System.Runtime.Versioning;

using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels;

namespace XsltEditor.Views.Windows;

[SupportedOSPlatform("windows")]
public partial class MainView
{
    private readonly IMessenger _messenger;

    public MainView(MainViewModel viewModel, IMessenger messenger)
    {
        DataContext = viewModel;

        InitializeComponent();
        InitializeWebView();

        _messenger = messenger;
        _messenger.Subscribe<ThemeMessage>(OnThemeChanged);
    }

    private async void InitializeWebView()
    {
        var webView2Environment = await CoreWebView2Environment.CreateAsync(null, null,
            new CoreWebView2EnvironmentOptions
            {
                AreBrowserExtensionsEnabled = false
            });

        await WebView.EnsureCoreWebView2Async(webView2Environment);

        if (WebView.DataContext is MainViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.HtmlContent) && sender is MainViewModel viewModel)
        {
            WebView.NavigateToString(viewModel.HtmlContent);
        }
    }

    private void OnThemeChanged(ThemeMessage message)
    {
        WebView.CoreWebView2.NavigationCompleted -= CoreWebView2_NavigationCompleted;

        if (message.IsDark)
        {
            WebView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        }

        WebView.CoreWebView2.Reload();
    }

    private async void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        await WebView.CoreWebView2.ExecuteScriptAsync(@"
            (function() {
                document.querySelectorAll('body, body *').forEach(el =>
                {
                    el.style.color = 'white';
                    el.style.backgroundColor = 'transparent';

                    if (el.tagName === 'TABLE' || el.tagName === 'TD' || el.tagName === 'TH')
                    {
                        el.style.border = '1px solid white';
                    }
                });

                document.querySelectorAll('table').forEach(table =>
                {
                    table.style.color = 'white';
                    table.style.borderColor = 'white';
                    table.style.backgroundColor = 'transparent';
                });
            })();
        ");
    }

    protected override void OnClosed(EventArgs e)
    {
        _messenger.Unsubscribe<ThemeMessage>(OnThemeChanged);

        if (DataContext is IDisposable disposable)
        {
            disposable.Dispose();
        }

        base.OnClosed(e);
    }
}