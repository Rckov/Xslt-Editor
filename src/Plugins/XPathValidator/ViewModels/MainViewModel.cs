using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.XPath;
using XPathValidator.Models;
using XPathValidator.Services;
using XsltEditor.Sdk.Abstractions;

namespace XPathValidator.ViewModels;

internal class MainViewModel : INotifyPropertyChanged
{
    private readonly XPathEvaluatorService? _evaluator;

    public MainViewModel(IDocument document)
    {
        try
        {
            _evaluator = XPathEvaluatorService.Create(document.Content);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to parse XML: {ex.Message}";
        }
    }

    public string? Expression
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                EvaluateExpression(value);
                OnPropertyChanged();
            }
        }
    }

    public string? ErrorMessage
    {
        get;
        private set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<ExpressionResult> Results { get; } = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    private void EvaluateExpression(string? expression)
    {
        Results.Clear();
        ErrorMessage = string.Empty;

        if (_evaluator is null || string.IsNullOrWhiteSpace(expression))
        {
            return;
        }

        try
        {
            foreach (var result in _evaluator.Evaluate(expression))
            {
                Results.Add(result);
            }
        }
        catch (XPathException ex)
        {
            ErrorMessage = $"XPath error: {ex.Message}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? property = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}