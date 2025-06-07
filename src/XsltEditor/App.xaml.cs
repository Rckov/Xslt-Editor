using Microsoft.Extensions.DependencyInjection;

using System.Runtime.Versioning;
using System.Windows;

using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels;

namespace XsltEditor;

[SupportedOSPlatform("windows")]
public partial class App
{
    static App()
    {
        Services = ConfigureContainer();
    }

    public static IServiceProvider Services { get; }

    private static ServiceProvider ConfigureContainer()
    {
        var services = new ServiceCollection();

        services.RegisterViews();
        services.RegisterServices();

        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var settingsService = Services.GetRequiredService<ISettingsService>();
        var task = settingsService.LoadSettings();

        task.GetAwaiter().OnCompleted(() =>
        {
            var windowService = Services.GetRequiredService<IWindowService>();
            windowService.ShowWindow<MainViewModel>();
        });
    }
}