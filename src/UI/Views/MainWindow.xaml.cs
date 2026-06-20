using System.ComponentModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using XsltEditor.Models;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Abstractions.Themes;
using XsltEditor.ViewModels;

namespace XsltEditor.Views;

public partial class MainWindow
{
    private ThemeType _currentTheme;

    public MainWindow()
    {
        InitializeComponent();

        var messenger = App.Services.GetRequiredService<IMessenger>();
        var themeService = App.Services.GetRequiredService<IThemeService>();

        _currentTheme = themeService.CurrentTheme;

        messenger.Register<ThemeChangedMessage>(this, (_, m) => ApplyTheme(m.ThemeType));

        InitializeWebView();
    }

    private async void InitializeWebView()
    {
        var environment = await CoreWebView2Environment.CreateAsync(
            null,
            Path.GetTempPath(),
            new CoreWebView2EnvironmentOptions { AreBrowserExtensionsEnabled = false });

        await WebView.EnsureCoreWebView2Async(environment);

        if (DataContext is MainViewModel vm)
        {
            vm.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(MainViewModel.HtmlContent) && sender is MainViewModel { HtmlContent: { } html })
        {
            WebView.NavigateToString(html);
        }
    }

    private void ApplyTheme(ThemeType themeType)
    {
        _currentTheme = themeType;

        if (WebView.CoreWebView2 is null)
        {
            return;
        }

        WebView.CoreWebView2.NavigationCompleted -= OnNavigationCompleted;
        WebView.CoreWebView2.NavigationCompleted += OnNavigationCompleted;

        if (themeType is ThemeType.Dark)
        {
            InjectDarkStyles();
        }
        else
        {
            RemoveDarkStyles();
        }
    }

    private async void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        if (_currentTheme is ThemeType.Dark)
        {
            InjectDarkStyles();
        }
    }

    private async void InjectDarkStyles()
    {
        if (WebView.CoreWebView2 is null)
        {
            return;
        }

        await WebView.CoreWebView2.ExecuteScriptAsync(
            "var style = document.getElementById('__dark') || document.createElement('style');"
            + "style.id = '__dark';"
            + "style.textContent = 'body{color:#e0e0e0!important;background-color:#1e1e1e!important}"
            + "table,td,th{border-color:#555!important}a{color:#6ea8fe!important}';"
            + "document.head.appendChild(style);");
    }

    private async void RemoveDarkStyles()
    {
        if (WebView.CoreWebView2 is null)
        {
            return;
        }

        await WebView.CoreWebView2.ExecuteScriptAsync(
            "document.getElementById('__dark')?.remove();");
    }

    private void OnCloseClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}