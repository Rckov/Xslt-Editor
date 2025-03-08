using Microsoft.Extensions.DependencyInjection;

using System.Windows;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services.Implementation;

internal class WindowService : IWindowService
{
    private readonly IServiceProvider _serviceProvider;

    public WindowService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ShowWindow<TWindow>() where TWindow : Window
    {
        var window = _serviceProvider.GetRequiredService<TWindow>();
        window.Show();
    }

    public void ShowDialogWindow<TWindow>() where TWindow : Window
    {
        var window = _serviceProvider.GetRequiredService<TWindow>();
        window.ShowDialog();
    }
}