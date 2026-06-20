using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using XsltEditor.Common.Attributes;
using XsltEditor.Services.Abstractions;

namespace XsltEditor.Services;

public class WindowService(IServiceProvider service) : IWindowService
{
    public T? ShowWindow<T>(T? context = null, bool dialog = false) where T : class
    {
        var window = GetWindow(context);

        if (dialog)
        {
            window.ShowDialog();
            return context;
        }

        window.Show();
        return context;
    }

    private Window GetWindow<T>(T? context) where T : class
    {
        context ??= service.GetRequiredService<T>();

        if (Attribute.GetCustomAttribute(typeof(T), typeof(WindowAttribute)) is not WindowAttribute attr)
        {
            throw new InvalidOperationException($"Window type not specified for {typeof(T).Name}");
        }

        var window = (Window)service.GetRequiredService(attr.WindowType);
        window.DataContext = context;

        return window;
    }
}