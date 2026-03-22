using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

using System.Text.Json.Serialization;
using System.Windows.Media;

namespace XsltEditor.Models;

internal class SnippetData(string text) : ICompletionData
{
	[JsonIgnore] public ImageSource? Image => null;
	public string Text => text;
	[JsonIgnore] public object Content => text;
	[JsonIgnore] public object Description => $"Insert <{text}> element";
	[JsonIgnore] public double Priority => 0;

	public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionEventArgs)
	{
		var offset = completionSegment.Offset;

		if (offset > 0 && textArea.Document.GetCharAt(offset - 1) is '<')
		{
			offset--;
		}

		var length = completionSegment.EndOffset - offset;
		var openTag = $"<{text}>";
		var closeTag = $"</{text}>";

		textArea.Document.Replace(offset, length, $"{openTag}{closeTag}");
		textArea.Caret.Offset = offset + openTag.Length;
	}
}