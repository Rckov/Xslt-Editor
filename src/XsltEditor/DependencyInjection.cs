using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;

using XsltEditor.Services;
using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels;
using XsltEditor.Views;

namespace XsltEditor;

internal static class DependencyInjection
{
    public static void RegisterViews(this IServiceCollection services)
    {
        var views = new Dictionary<Type, Type>();

        services.RegisterDialog<MainViewModel, MainWindow>(viewMap: views);

        services.AddSingleton<IDictionary<Type, Type>>(views);
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        services.AddSingleton<IWindowFactory, WindowFactory>();
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddTransient<IFileService, FileDialogService>();
        services.AddTransient<IXmlTransformService, XmlTransformService>();
        services.AddTransient<ISettingsService>(_ => new SettingsService("settings.json"));
    }

    private static void RegisterDialog<TViewModel, TDialog>(this IServiceCollection services, Dictionary<Type, Type> viewMap)
        where TViewModel : class
        where TDialog : class
    {
        ArgumentNullException.ThrowIfNull(viewMap);
        
        services.AddTransient<TViewModel>();
        services.AddTransient<TDialog>();

        viewMap[typeof(TViewModel)] = typeof(TDialog);
    }
}