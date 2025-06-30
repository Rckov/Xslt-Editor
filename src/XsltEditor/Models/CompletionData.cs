using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace XsltEditor.Models;

[SupportedOSPlatform("windows")]
internal class CompletionData(string text) : ICompletionData
{
    [JsonIgnore] public string OpenTag => $"<{Text}>";

    [JsonIgnore] public string CloseTag => $"</{Text}>";
    [JsonIgnore] public ImageSource? Image => null;

    public string Text { get; } = text;

    [JsonIgnore] public object Content => Text;

    [JsonIgnore] public object Description => $"Insert {Text}";

    [JsonIgnore] public double Priority => 0;

    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
    {
        textArea.Document.Replace(completionSegment.Offset - 1, 1, string.Empty);
        textArea.Document.Replace(completionSegment, $"{OpenTag}{CloseTag}");

        textArea.Caret.Offset -= CloseTag.Length;
    }
}