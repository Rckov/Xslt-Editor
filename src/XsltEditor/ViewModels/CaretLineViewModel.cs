using System.Windows.Input;

using XsltEditor.Infrastructure;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels.Base;

namespace XsltEditor.ViewModels;

public class CaretLineViewModel : BaseViewModel
{
    private readonly IMessenger _messenger;

    public CaretLineViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }

    public string? Line
    {
        get;
        set => Set(ref field, value);
    }

    public ICommand? GoToCommand { get; private set; }
    public ICommand? CloseCommand { get; set; }

    protected override void InitializeCommands()
    {
        GoToCommand = new RelayCommand(GoToLine);
    }

    private void GoToLine()
    {
        try
        {
            if (int.TryParse(Line, out var lineNumber) && lineNumber > 0)
            {
                _messenger.Send(new CaretLineMessage(lineNumber));
            }

            CloseCommand?.Execute(null);
        }
        catch (Exception ex)
        {
            LogError($"Error {nameof(CaretLineViewModel)}", ex);
        }
    }
}