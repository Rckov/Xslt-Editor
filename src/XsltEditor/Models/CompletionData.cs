using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace XsltEditor.Models;

[SupportedOSPlatform("windows")]
internal class CompletionData(string text) : ICompletionData
{
    private string _openTag => $"<{Text}>";
    private string _closeTag => $"</{Text}>";

    [JsonIgnore] public ImageSource? Image => null;

    public string Text { get; } = text;

    [JsonIgnore] public object Content => Text;

    [JsonIgnore] public object Description => $"Insert {Text}";

    [JsonIgnore] public double Priority => 0;

    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
    {
        textArea.Document.Replace(completionSegment.Offset - 1, 1, string.Empty);
        textArea.Document.Replace(completionSegment, $"{_openTag}{_closeTag}");

        textArea.Caret.Offset -= _closeTag.Length;
    }
}