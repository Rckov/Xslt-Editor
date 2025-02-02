using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace XsltEditor.Models;

[SupportedOSPlatform("windows")]
public class CompletionData : ICompletionData
{
    public CompletionData(string text)
    {
        Text = text;
    }

    [JsonIgnore] public ImageSource? Image => null;

    public string Text { get; }

    [JsonIgnore] 
    public object Content => Text;

    [JsonIgnore] 
    public object Description => $"Insert {Text}";

    [JsonIgnore] 
    public double Priority => 0;

    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
    {
        textArea.Document.Replace(completionSegment, Text);
    }
}