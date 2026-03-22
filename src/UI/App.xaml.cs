using Microsoft.Extensions.DependencyInjection;

using System.Windows;

using XsltEditor.Extensions;
using XsltEditor.Services.Abstractions;
using XsltEditor.Services.Abstractions.Themes;
using XsltEditor.ViewModels;
using XsltEditor.Views.Startup;

namespace XsltEditor;

public partial class App : Application
{
	static App()
	{
		Services = ConfigureServices();
	}

	public static IServiceProvider Services { get; }

	protected override async void OnStartup(StartupEventArgs e)
	{
		InitializeTheme();

		var splash = new SplashWindow();
		splash.Show();

		try
		{
			await Services
				.GetRequiredService<ITransformService>()
				.WarmupAsync();

			Services
				.GetRequiredService<IWindowService>()
				.ShowWindow<MainViewModel>();
		}
		finally
		{
			splash.Close();
		}
	}

	private static void InitializeTheme()
	{
		var settings = Services.GetRequiredService<ISettingsService>();

		Services
			.GetRequiredService<IThemeService>()
			.SetTheme(settings.Settings.ThemeType);
	}

	private static IServiceProvider ConfigureServices()
	{
		var services = new ServiceCollection();

		services.AddUI();
		services.AddServices();

		return services.BuildServiceProvider();
	}
}