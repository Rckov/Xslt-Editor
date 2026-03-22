using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;

using System.ComponentModel;
using System.IO;
using System.Windows;

using XsltEditor.Models;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Abstractions.Themes;
using XsltEditor.ViewModels;

namespace XsltEditor.Views;

public partial class MainWindow : Window
{
	private ThemeType _currentTheme;

	public MainWindow()
	{
		InitializeComponent();

		IMessenger messenger = App.Services.GetRequiredService<IMessenger>();
		IThemeService themeService = App.Services.GetRequiredService<IThemeService>();

		_currentTheme = themeService.CurrentTheme;

		messenger.Register<ThemeChangedMessage>(this, (_, m) => ApplyTheme(m.ThemeType));

		InitializeWebView();
	}

	private async void InitializeWebView()
	{
		CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(
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
		if (e.PropertyName is nameof(MainViewModel.HtmlContent) && sender is MainViewModel vm && vm.HtmlContent is { } html)
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

		if (themeType is ThemeType.Dark)
		{
			WebView.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
		}

		if (WebView.CoreWebView2.Source is not "about:blank")
		{
			WebView.CoreWebView2.Reload();
		}
	}

	private async void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		if (_currentTheme is not ThemeType.Dark)
		{
			return;
		}

		await WebView.CoreWebView2.ExecuteScriptAsync("""
			(() => {
				const style = document.getElementById('__dark') ?? document.createElement('style');
				style.id = '__dark';
				style.textContent = `
					body, body * { color: white !important; background-color: transparent !important; }
					table, td, th { border-color: white !important; }
				`;
				document.head.appendChild(style);
			})();
			""");
	}

	private void OnCloseClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}