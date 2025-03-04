using Microsoft.Extensions.DependencyInjection;

using System.Runtime.Versioning;
using System.Windows;

using XsltEditor.Services;
using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels;
using XsltEditor.Views.Windows;

namespace XsltEditor;

[SupportedOSPlatform("windows")]
public partial class App
{
    public App()
    {
        Services = ConfigureServices(new ServiceCollection());
    }

    public static IServiceProvider Services { get; private set; } = null!;

    private static ServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICompletionDataService, CompletionDataService>();
        services.AddSingleton<IWindowService, WindowService>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainView>();

        services.AddTransient<CompletionViewModel>();
        services.AddTransient<CompletionView>();

        services.AddTransient<GoToLineViewModel>();
        services.AddTransient<GoToLineView>();

        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var windowService = Services.GetRequiredService<IWindowService>();
        windowService?.ShowWindow<MainView>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (Services is IDisposable disposable)
        {
            disposable.Dispose();
        }

        base.OnExit(e);
    }
}