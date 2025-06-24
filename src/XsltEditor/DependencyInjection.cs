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
        services.RegisterView<MainViewModel, MainWindow>();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IWindowService, WindowService>();
        services.AddTransient<IResourceOperationsService, ResourceOperationsService>();
        services.AddTransient<IFileDialogService, FileDialogService>();
        services.AddTransient<IDocumentStorageService, DocumentStorageService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IXmlTransformService, XmlTransformService>();
        services.AddSingleton<ISettingsService, SettingsService>();

        services.AddTransient<IMessenger>(sp => WeakReferenceMessenger.Default);
    }

    public static void RegisterView<TViewModel, TView>(this IServiceCollection services)
        where TViewModel : class
        where TView : class
    {
        services.AddTransient<TViewModel>();
        services.AddTransient<TView>();
    }
}