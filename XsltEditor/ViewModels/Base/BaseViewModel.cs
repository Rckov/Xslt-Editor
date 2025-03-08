using System.Diagnostics;
using System.Runtime.CompilerServices;

using XsltEditor.Models.Base;

namespace XsltEditor.ViewModels.Base;

public abstract class BaseViewModel : ObservableObject
{
    protected BaseViewModel()
    {
        InitializeCommands();
    }

    protected virtual void InitializeCommands()
    {
    }

    protected void LogInfo(string message, [CallerMemberName] string? method = null)
    {
        Debug.WriteLine(
            $"Method: {method}\r\n" +
            $"Message: {message}"
        );
    }

    protected void LogError(string message, Exception? exception = null, [CallerMemberName] string? method = null)
    {
        Debug.WriteLine(
            $"Method: {method}\r\n" +
            $"Message: {message}\r\n" +
            $"Exception: {exception?.Message ?? "No exception"}"
        );
    }
}