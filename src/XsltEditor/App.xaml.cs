using Microsoft.Extensions.DependencyInjection;

using System.Windows;

using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels;

namespace XsltEditor;

public partial class App
{
    static App()
    {
        Services = ConfigureServices();
    }

    public static IServiceProvider Services { get; }

    private static IServiceProvider ConfigureServices()
    {
        var service = new ServiceCollection();

        service.RegisterViews();
        service.RegisterServices();

        return service.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var windowService = Services.GetRequiredService<IWindowService>();
        windowService.ShowWindow<MainViewModel>();
    }
}