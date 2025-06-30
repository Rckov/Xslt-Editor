using Microsoft.Extensions.DependencyInjection;

using System.IO;
using System.Windows;

using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels;

namespace XsltEditor;

public partial class App
{
    static App()
    {
        Services = ConfigureServices();
        AppDirectory = GetApplicationDirectory();
    }

    public static string AppDirectory { get; }

    public static IServiceProvider Services { get; }

    private static IServiceProvider ConfigureServices()
    {
        var service = new ServiceCollection();

        service.RegisterServices();
        service.RegisterViews();

        return service.BuildServiceProvider();
    }

    private static string GetApplicationDirectory()
    {
        const Environment.SpecialFolder data = Environment.SpecialFolder.ApplicationData;
        var directory = Path.Combine(Environment.GetFolderPath(data), "Xslt Editor");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return directory;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var windowService = Services.GetRequiredService<IWindowService>();
        windowService.ShowWindow<MainViewModel>();
    }
}