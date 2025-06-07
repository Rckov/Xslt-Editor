using Microsoft.Extensions.DependencyInjection;

using System.Windows;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class WindowService(IWindowFactory factory, IServiceProvider provider) : IWindowService
{
    public void ShowWindow<TViewModel>() where TViewModel : class
    {
        GetWindow<TViewModel>().Show();
    }

    public bool? ShowDialog<TViewModel>() where TViewModel : class
    {
        return GetWindow<TViewModel>().ShowDialog();
    }

    public void ShowMessage(string message, string caption)
    {
        MessageBox.Show(message, caption);
    }

    private Window GetWindow<TViewModel>() where TViewModel : class
    {
        var viewModel = provider.GetRequiredService(typeof(TViewModel));
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