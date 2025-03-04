using Microsoft.Extensions.DependencyInjection;

using System.Runtime.Versioning;

using XsltEditor.Services.Interfaces;
using XsltEditor.Views.UserControls;

namespace XsltEditor.Services;

[SupportedOSPlatform("windows")]
internal class WindowService(IServiceProvider serviceProvider) : IWindowService
{
    public void ShowWindow<TWindow>() where TWindow : Window
    {
        var window = serviceProvider.GetRequiredService<TWindow>();
        window.Show();
    }

    public void ShowDialogWindow<TWindow>() where TWindow : Window
    {
        var window = serviceProvider.GetRequiredService<TWindow>();
        window.ShowDialog();
    }
}