using Microsoft.Web.WebView2.Core;

using System.ComponentModel;
using System.IO;
using System.Runtime.Versioning;

using XsltEditor.Helpers;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels;

namespace XsltEditor.Views.Windows;

[SupportedOSPlatform("windows")]
public partial class MainView
{
    private readonly IMessenger _messenger;

    public MainView(MainViewModel viewModel, IMessenger messenger, IThemeManager themeManager)
    {
        DataContext = viewModel;

        _messenger = messenger;
        _messenger.Subscribe<ThemeMessage>(OnThemeChanged);

        InitializeComponent();
        InitializeWebView(themeManager);
    }

    private async void InitializeWebView(IThemeManager themeManager)
    {
        var webView2Environment = await CoreWebView2Environment.CreateAsync(null, Path.GetTempPath(),
            new CoreWebView2EnvironmentOptions
            {
                AreBrowserExtensionsEnabled = false,
                AdditionalBrowserArguments = "--edge-webview-is-background --single-process --disable-site-isolation-trials --disable-gpu"
            });

        await WebView.EnsureCoreWebView2Async(webView2Environment);

        if (WebView.DataContext is MainViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            /// Ensures the theme change is applied reliably.
            /// Since theme change message registration happens later, WebView might not subscribe to
            /// <see cref="CoreWebView2_NavigationCompleted"/> in time. This guarantees the theme is applied correctly.
            OnThemeChanged(new ThemeMessage(themeManager.CurrentTheme));
        }

        WebView.CoreWebView2.SaveAsUIShowing += CoreWebView2_SaveAsUIShowing;
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

        /// During WebView initialization, if no Source is set, calling Reload() may throw an exception.
        /// Exception: "CoreWebView2 members cannot be accessed after the WebView2 control is disposed"
        /// https://github.com/MicrosoftEdge/WebView2Feedback/issues/2872
        if (WebView.CanGoBack)
        {
            WebView.CoreWebView2.Reload();
        }
    }

    private async void CoreWebView2_SaveAsUIShowing(object? sender, CoreWebView2SaveAsUIShowingEventArgs e)
    {
        e.Cancel = true;

        if (DataContext is MainViewModel viewModel && !string.IsNullOrEmpty(viewModel.HtmlContent))
        {
            var path = Dialog.SaveFile("Save HTML", ".html");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            await File.WriteAllTextAsync(path, viewModel.HtmlContent);
        }
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