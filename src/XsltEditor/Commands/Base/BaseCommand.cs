using System.Windows.Input;

namespace XsltEditor.Commands.Base;

internal abstract class BaseCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public abstract void Execute(object? parameter);

    public abstract bool CanExecute(object? parameter);
}

internal class RelayCommand : BaseCommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public override void Execute(object? parameter) => _execute(parameter);

    public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
}