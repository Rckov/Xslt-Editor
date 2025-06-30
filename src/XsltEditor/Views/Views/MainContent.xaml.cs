using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;

using System.ComponentModel;
using System.IO;

using XsltEditor.Models.Enums;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels;

namespace XsltEditor.Views.Views;

public partial class MainContent
{
    public MainContent()
    {
        InitializeComponent();

        var messenger = App.Services.GetRequiredService<IMessenger>();
        var theme = App.Services.GetRequiredService<IThemeService>();

        messenger.Register<ThemeChangedMessage>(this, (_, m) => OnThemeChanged(m.Value));

        InitializeComponent();
        InitializeWebView(theme.CurrentThemeType);
    }

    private async void InitializeWebView(ThemeType themeType)
    {
        var webView2Environment = await CoreWebView2Environment.CreateAsync(null, Path.GetTempPath(),
            new CoreWebView2EnvironmentOptions
            {
                AreBrowserExtensionsEnabled = false,
            });

        await WebView.EnsureCoreWebView2Async(webView2Environment);

        if (WebView.DataContext is MainViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            /// Ensures the theme change is applied reliably.
            /// Since theme change message registration happens later, WebView might not subscribe to
            /// <see cref="CoreWebView2_NavigationCompleted"/> in time. This guarantees the theme is applied correctly.
            OnThemeChanged(themeType);
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.HtmlContent) && sender is MainViewModel viewModel)
        {
            WebView.NavigateToString(viewModel.HtmlContent);
        }
    }

    private void OnThemeChanged(ThemeType themeType)
    {
        WebView.CoreWebView2.NavigationCompleted -= CoreWebView2_NavigationCompleted;

        if (themeType == ThemeType.Dark)
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
}