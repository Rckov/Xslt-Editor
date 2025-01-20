using System.Runtime.Versioning;
using System.Windows.Input;

namespace XsltEditor.Core;

[SupportedOSPlatform("windows")]
internal class RelayCommand : ICommand
{
    private readonly Func<object?, bool>? _canExecute;
    private readonly Action<object?> _execute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;

        CommandManager.RequerySuggested += (_, _) => RaiseCanExecuteChanged();
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}