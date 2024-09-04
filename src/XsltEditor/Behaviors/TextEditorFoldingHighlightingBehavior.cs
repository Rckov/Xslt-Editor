using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using Microsoft.Xaml.Behaviors;

using System.IO;
using System.Reflection;
using System.Xml;

namespace XsltEditor.Behaviors;

internal class TextEditorFoldingHighlightingBehavior : Behavior<TextEditor>
{
    private FoldingManager? _foldingManager;
    private XmlFoldingStrategy? _foldingStrategy;

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null)
        {
            return;
        }

        LoadHighlightingDefinitionFromResource("XsltEditor.Resources.Highlighting.XSLMode.xshd");

        _foldingManager = FoldingManager.Install(AssociatedObject.TextArea);
        _foldingStrategy = new XmlFoldingStrategy();

        AssociatedObject.TextChanged += OnTextEditorTextChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (_foldingManager is not null)
        {
            FoldingManager.Uninstall(_foldingManager);
            _foldingManager = null;
        }
    }

    private void UpdateFolding()
    {
        if (_foldingManager is null || _foldingStrategy is null)
        {
            return;
        }

        _foldingStrategy.UpdateFoldings(_foldingManager, AssociatedObject.Document);
    }

    private void OnTextEditorTextChanged(object? sender, EventArgs e)
    {
        UpdateFolding();
    }

    private void LoadHighlightingDefinitionFromResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using Stream? stream = assembly.GetManifestResourceStream(resourceName);

        if (stream is null)
        {
            throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        }

        using var reader = XmlReader.Create(stream);
        var highlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);

        HighlightingManager.Instance.RegisterHighlighting("XSL-Mode", [".xsl", ".xml"], highlightingDefinition);

        AssociatedObject.SyntaxHighlighting = highlightingDefinition;
    }
}