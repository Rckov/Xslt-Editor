using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using Microsoft.Xaml.Behaviors;

using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;
using System.Xml;

using XsltEditor.Tools;

namespace XsltEditor.Core.Behaviors;

[SupportedOSPlatform("windows")]
internal class TextEditorHighlightingBehavior : Behavior<TextEditor>
{
    public static Dictionary<ThemeType, string> _highlightings = new()
    {
        { ThemeType.Dark, "XsltEditor.Resources.Xshd.DarkMode.xshd" },
        { ThemeType.Light, "XsltEditor.Resources.Xshd.LightMode.xshd" }
    };

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null)
        {
            return;
        }

        ThemeManager.ThemeChanged += LoadHighlighting;
        LoadHighlighting(ThemeManager.CurrentTheme);
    }

    protected override void OnDetaching()
    {
        ThemeManager.ThemeChanged -= LoadHighlighting;
        base.OnDetaching();
    }

    private void LoadHighlighting(ThemeType type)
    {
        try
        {
            var highlighting = _highlightings[type];
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(highlighting);

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