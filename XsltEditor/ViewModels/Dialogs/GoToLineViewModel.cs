using System.Windows.Input;

using XsltEditor.Infrastructure;
using XsltEditor.Models.Base;
using XsltEditor.ViewModels.Main;

namespace XsltEditor.ViewModels;

public class GoToLineViewModel : ObservableObject
{
    private readonly MainViewModel? _viewModel;

    public Action? CloseWindow { get; set; }

    public string? LineNumber
    {
        get;
        set => Set(ref field, value);
    }

    public ICommand GoToCommand { get; private set; } = null!;

    public GoToLineViewModel(MainViewModel? viewModel)
    {
        _viewModel = viewModel;
        InitCommands();
    }

    private void InitCommands()
    {
        GoToCommand = new RelayCommand(GoToLine);
    }

    private void GoToLine(object? parameter)
    {
        if (_viewModel?.ActiveDocument is null || string.IsNullOrEmpty(LineNumber))
        {
            return;
        }

        if (int.TryParse(LineNumber, out var lineNumber) && lineNumber > 0)
        {
            _viewModel.ActiveDocument.Line = lineNumber;
        }

        CloseWindow?.Invoke();
    }
}