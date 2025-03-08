using System.Windows.Input;

namespace XsltEditor.Infrastructure;

public class RelayCommand : ICommand
{
    private readonly Predicate<object?>? _canExecute;
    private readonly Action<object?> _execute;

    public RelayCommand(Action execute) : this(_ => execute(), null)
    { }

    public RelayCommand(Action<object?> execute) : this(execute, null)
    { }

    public RelayCommand(Action execute, Func<bool> canExecute) : this(_ => execute(), _ => canExecute())
    { }

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }
}