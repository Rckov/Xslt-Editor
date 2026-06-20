using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using XsltEditor.Common.Attributes;
using XsltEditor.Views;

namespace XsltEditor.ViewModels;

[Window(typeof(GoToLineWindow))]
public partial class GoToLineViewModel(int currentLine) : ObservableObject
{
    [ObservableProperty] private string _lineText = currentLine.ToString();

    public int? Result { get; private set; }

    [RelayCommand(CanExecute = nameof(CanGo))]
    private void Go()
    {
        if (int.TryParse(LineText, out var value))
        {
            Result = value;
        }
    }

    private bool CanGo()
    {
        return !string.IsNullOrEmpty(LineText);
    }

    partial void OnLineTextChanged(string value)
    {
        GoCommand.NotifyCanExecuteChanged();
    }
}