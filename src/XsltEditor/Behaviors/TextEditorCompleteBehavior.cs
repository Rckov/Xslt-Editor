using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

using Microsoft.Xaml.Behaviors;

using System.Windows.Input;
using System.Windows.Media;

namespace XsltEditor.Behaviors;

internal class TextEditorCompleteBehavior : Behavior<TextEditor>
{
    private CompletionWindow? _completionWindow;

    private readonly IList<CompletionData> _completionDataList =
    [
        new("html"),
        new("head"),
        new("title"),
        new("body"),
        new("div"),
        new("span"),
        new("img"),
        new("table"),
        new("style"),
        new("xsl:value-of"),
        new("xsl:choose"),
        new("xsl:template"),
        new("xsl:if"),
        new("xsl:text"),
        new("xsl:for-each"),
        new("xsl:apply-templates"),
        new("xsl:call-template"),
        new("xsl:when"),
        new("xsl:otherwise"),
        new("xsl:variable"),
        new("xsl:param")
    ];

    protected override void OnAttached()
    {
        base.OnAttached();
        SubscribeToEvents();
    }

    protected override void OnDetaching()
    {
        UnsubscribeFromEvents();
        base.OnDetaching();
    }

    private void SubscribeToEvents()
    {
        AssociatedObject.TextArea.TextEntered += OnTextEntered;
        AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
    }

    private void UnsubscribeFromEvents()
    {
        AssociatedObject.TextArea.TextEntered -= OnTextEntered;
        AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
    }

    private void OnTextEntered(object sender, TextCompositionEventArgs e)
    {
        if (e.Text == "<")
        {
            ShowCompletionWindow();
        }
    }

    private void ShowCompletionWindow()
    {
        _completionWindow = new CompletionWindow(AssociatedObject.TextArea);
        var data = _completionWindow.CompletionList.CompletionData;

        foreach (var completionData in _completionDataList)
        {
            data.Add(completionData);
        }

        _completionWindow.Closed += (o, args) => _completionWindow = null;
        _completionWindow.Show();
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (_completionWindow != null && e.Key == Key.Enter)
        {
            _completionWindow.CompletionList.RequestInsertion(e);
            e.Handled = true;
        }
    }

    private class CompletionData(string text) : ICompletionData
    {
        public ImageSource? Image => null;
        public string Text { get; } = text;
        public object Content => Text;
        public object Description => $"Insert {Text}";
        public double Priority => 0;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, Text);
        }
    }
}