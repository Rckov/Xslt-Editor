using System.Runtime.Versioning;
using System.Windows.Input;

using XsltEditor.Tools.Commands.Base;

namespace XsltEditor.Tools.Commands;

[SupportedOSPlatform("windows")]
internal class RelayCommand : BaseCommand
{
    private readonly Func<object?, bool>? _canExecute;
    private readonly Action<object?> _execute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;

        CommandManager.RequerySuggested += (_, _) => RaiseCanExecuteChanged();
    }

    public override void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public override bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void RaiseCanExecuteChanged()
    {
        OnCanExecuteChanged();
    }
}