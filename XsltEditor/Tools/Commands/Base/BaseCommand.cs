using System.Runtime.Versioning;
using System.Windows.Input;

namespace XsltEditor.Tools.Commands.Base;

[SupportedOSPlatform("windows")]
internal abstract class BaseCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public abstract void Execute(object? parameter);

    public abstract bool CanExecute(object? parameter);

    protected void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}