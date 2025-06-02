using System.Windows;

namespace XsltEditor.Services.Interfaces;

internal interface IWindowService
{
    void Show<TViewModel>(object? parameter = null) where TViewModel : class;

    bool? ShowDialog<TViewModel>(object? parameter = null) where TViewModel : class;
}

internal interface IWindowFactory
{
    Window CreateWindow<TViewModel>(TViewModel viewModel) where TViewModel : class;
}

internal interface IParameterReceiver
{
    void SetParameter(object? parameter = null);
}