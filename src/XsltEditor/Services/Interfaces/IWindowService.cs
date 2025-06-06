using System.Windows;

namespace XsltEditor.Services.Interfaces;

internal interface IWindowService
{
    void ShowWindow<TViewModel>() where TViewModel : class;

    bool? ShowDialog<TViewModel>() where TViewModel : class;

    MessageBoxResult ShowMessage(string message, string caption);
}

internal interface IWindowFactory
{
    Window CreateWindow<TViewModel>(TViewModel viewModel) where TViewModel : class;
}