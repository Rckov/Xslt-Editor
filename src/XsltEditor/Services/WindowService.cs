using Microsoft.Extensions.DependencyInjection;

using System.Windows;

using XsltEditor.Common.Attributes;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class WindowService(IServiceProvider provider) : IWindowService
{
    public void ShowWindow<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var window = GetWindow<TViewModel>();
        window.Show();
    }

    public void ShowDialog<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var window = GetWindow<TViewModel>();
        window.ShowDialog();
    }

    private Window GetWindow<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var viewModel = provider.GetRequiredService<TViewModel>();

        if (viewModel is IParameterReceiver receiver)
        {
            receiver.SetParameter(parameter);
        }

        if (Attribute.GetCustomAttribute(typeof(TViewModel), typeof(WindowAttribute)) is not WindowAttribute windowType || windowType.WindowType == null)
        {
            throw new InvalidOperationException("Window type not specified for ViewModel");
        }

        var window = (Window)provider.GetRequiredService(windowType.WindowType);
        window.DataContext = viewModel;

        return window;
    }
}