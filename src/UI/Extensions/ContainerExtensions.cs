using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using XsltEditor.Services;
using XsltEditor.Services.Abstractions;
using XsltEditor.Services.Abstractions.Themes;
using XsltEditor.Services.Themes;
using XsltEditor.ViewModels;
using XsltEditor.Views;

namespace XsltEditor.Extensions;

public static class ContainerExtensions
{
    extension(IServiceCollection services)
    {
        public void AddUi()
        {
            services.AddView<MainViewModel, MainWindow>();
            services.AddView<SettingsViewModel, SettingsWindow>();
            services.AddView<SnippetViewModel, SnippetWindow>();
            services.AddView<GoToLineViewModel, GoToLineWindow>();
            services.AddTransient<DocumentViewModel>();
        }

        public void AddServices()
        {
            services.AddTransient<IWindowService, WindowService>();
            services.AddTransient<IDocumentFactory, DocumentFactory>();
            services.AddTransient<IFileDialogService, FileDialogService>();
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
            services.AddSingleton<IThemeProvider, ThemeProvider>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<ISnippetService, SnippetService>();
            services.AddSingleton<ITransformService, TransformService>();
            services.AddSingleton<IPluginService, PluginService>();
        }

        private void AddView<TViewModel, TView>()
            where TViewModel : class
            where TView : class
        {
            services.AddTransient<TViewModel>();
            services.AddTransient<TView>();
        }
    }
}