using Microsoft.Extensions.DependencyInjection;

using System.Windows.Input;

using XsltEditor.Infrastructure;
using XsltEditor.Models.Base;

namespace XsltEditor.ViewModels;

public class GoToLineViewModel : ObservableObject
{
    private readonly MainViewModel? _viewModel;

    public GoToLineViewModel(IServiceProvider service)
    {
        _viewModel = service.GetService<MainViewModel>();

        LineNumber = "0";
        GoToCommand = new RelayCommand(GoToLine);
    }

    public Action? CloseWindow { get; set; }

    public string? LineNumber
    {
        get;
        set => Set(ref field, value);
    }

    public ICommand GoToCommand { get; }

    private void GoToLine(object? parameter)
    {
        if (_viewModel?.ActiveDocument is null)
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