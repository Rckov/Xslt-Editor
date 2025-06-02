using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using System.ComponentModel.DataAnnotations;

using XsltEditor.Models.Messages;

namespace XsltEditor.ViewModels;

internal partial class CaretViewModel : ObservableRecipient
{
    [ObservableProperty] 
    private string? _line;

    public event Action<bool>? CloseRequest;

    [RelayCommand]
    private void ChangeCaretLine()
    {
        if (int.TryParse(Line, out var lineNumber) && lineNumber > 0)
        {
            Messenger.Send(new CaretChangedMessage(lineNumber));
        }

        CloseRequest?.Invoke(true);
    }
}