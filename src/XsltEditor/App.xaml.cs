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
        Services = ConfigureContainer()!;
    }

    public static IServiceProvider Services { get; }
    public static string BaseDirectory => AppContext.BaseDirectory;

    private static ServiceProvider ConfigureContainer()
    {
        var services = new ServiceCollection();

        services.RegisterViews();
        services.RegisterServices();

        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var service = Services.GetRequiredService<IWindowService>();
        service.Show<MainViewModel>();
    }
}