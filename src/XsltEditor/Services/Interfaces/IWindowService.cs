namespace XsltEditor.Services.Interfaces;

internal interface IWindowService
{
    void ShowWindow<TViewModel>(object? parameter = null) where TViewModel : class;

    void ShowDialog<TViewModel>(object? parameter = null) where TViewModel : class;
}

internal interface IParameterReceiver
{
    void SetParameter(object? parameter = null);
}