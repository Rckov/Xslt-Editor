using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using Microsoft.Xaml.Behaviors;

using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;
using System.Xml;

namespace XsltEditor.Tools.Behaviors.TextEditor;

[SupportedOSPlatform("windows")]
internal class TextEditorHighlightingBehavior : Behavior<ICSharpCode.AvalonEdit.TextEditor>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null)
        {
            return;
        }

        LoadHighlighting();
    }

    private void LoadHighlighting()
    {
        try
        {
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("XsltEditor.Resources.Xshd.HighlightingMode.xshd");

            if (stream == null)
            {
                return;
            }

            using var reader = XmlReader.Create(stream);
            var highlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);

            AssociatedObject.SyntaxHighlighting = highlightingDefinition;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading highlighting definition: {ex.Message}");
        }
    }
}