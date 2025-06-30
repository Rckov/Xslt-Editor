using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using XsltEditor.Common.Attributes;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;
using XsltEditor.Views.Dialogs;

namespace XsltEditor.ViewModels;

[Window(typeof(CaretDialog))]
internal partial class CaretViewModel(IMessenger messenger) : ObservableObject, IParameterReceiver
{
    [ObservableProperty] private Guid? _id;
    [ObservableProperty] private string? _line;

    public void SetParameter(object? parameter = null)
    {
        if (parameter is Guid id)
        {
            Id = id;
        }
    }

    public event Action<bool>? CloseRequest;

    [RelayCommand]
    private void ChangeCaretLine()
    {
        if (int.TryParse(Line, out var lineNumber) && lineNumber > 0)
        {
            messenger.Send(new CaretChangedMessage(Id, lineNumber));
        }

        CloseRequest?.Invoke(true);
    }
}