using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;

using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Input;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Core.Behaviors;

[SupportedOSPlatform("windows")]
internal class TextEditorCompleteBehavior : Behavior<TextEditor>
{
    private CompletionWindow? _completionWindow;
    private IList<CompletionData>? _completionDataList;

    public TextEditorCompleteBehavior()
    {
        LoadCompletionData();
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (_completionDataList is null || _completionDataList.Count == 0)
        {
            return;
        }

        AssociatedObject.TextArea.TextEntered += OnTextEntered;
        AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.TextArea.TextEntered -= OnTextEntered;
        AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;

        base.OnDetaching();
    }

    private void LoadCompletionData()
    {
        var service = App.Services?.GetService<ICompletionDataService>();

        if (service == null)
        {
            _completionDataList = [];
            return;
        }

        _completionDataList = service.LoadCompletionData();
    }

    private void OnTextEntered(object sender, TextCompositionEventArgs e)
    {
        if (e.Text != "<")
        {
            return;
        }

        _completionWindow = new CompletionWindow(AssociatedObject.TextArea)
        {
            ResizeMode = ResizeMode.NoResize
        };

        var data = _completionWindow.CompletionList.CompletionData;

        foreach (var item in _completionDataList!)
        {
            data.Add(item);
        }

        _completionWindow.Closed += (_, _) => _completionWindow = null;
        _completionWindow.Show();
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (_completionWindow == null || e.Key != Key.Enter)
        {
            return;
        }

        _completionWindow.CompletionList.RequestInsertion(e);
        e.Handled = true;
    }
}