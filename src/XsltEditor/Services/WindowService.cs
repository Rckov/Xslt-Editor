using Microsoft.Extensions.DependencyInjection;

using System.Windows;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class WindowService(IWindowFactory factory, IServiceProvider provider) : IWindowService
{
    public void Show<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var window = GetWindow<TViewModel>(parameter);
        window.Show();
    }

    public bool? ShowDialog<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var window = GetWindow<TViewModel>(parameter);
        return window.ShowDialog();
    }

    private Window GetWindow<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var viewModel = provider.GetRequiredService(typeof(TViewModel));

        if (parameter != null && viewModel is IParameterReceiver receiver)
        {
            receiver.SetParameter(parameter);
        }

        return factory.CreateWindow(viewModel);
    }
}

internal class WindowFactory(IDictionary<Type, Type> views, IServiceProvider provider) : IWindowFactory
{
    public Window CreateWindow<TViewModel>(TViewModel viewModel) where TViewModel : class
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        var vmType = viewModel.GetType();

        if (!views.TryGetValue(vmType, out var viewType))
        {
            throw new InvalidOperationException($"View not registered for {vmType.Name}");
        }

        var window = (Window)provider.GetRequiredService(viewType);
        window.DataContext = viewModel;
        return window;
    }
}