using ICSharpCode.AvalonEdit.Folding;

using Microsoft.Xaml.Behaviors;

using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Media;

namespace XsltEditor.Tools.Behaviors.TextEditor;

[SupportedOSPlatform("windows")]
internal class TextEditorFoldingBehavior : Behavior<ICSharpCode.AvalonEdit.TextEditor>
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

        ApplyFoldingColors();

        _foldingManager = FoldingManager.Install(AssociatedObject.TextArea);
        _foldingStrategy = new XmlFoldingStrategy();

        AssociatedObject.Loaded += AssociatedObject_Loaded;
        AssociatedObject.TextChanged += OnTextEditorTextChanged;
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        if (AssociatedObject.TextArea.LeftMargins.Count > 0 &&
            AssociatedObject.TextArea.LeftMargins[1] is { } line)
        {
            line.Opacity = 0;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (_foldingManager is null)
        {
            return;
        }

        FoldingManager.Uninstall(_foldingManager);
        _foldingManager = null;
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

    private void ApplyFoldingColors()
    {
        var foldingBackgroundColor = GetColorFromResource("FoldingBackgroundColor");
        var foldingSelectBackgroundColor = GetColorFromResource("FoldingSelectBackgroundColor");
        var foldingSelectMarkerBackgroundColor = GetColorFromResource("FoldingSelectMarkerBackgroundColor");

        FoldingMargin.SetFoldingMarkerBackgroundBrush(AssociatedObject, new SolidColorBrush(foldingBackgroundColor));
        FoldingMargin.SetSelectedFoldingMarkerBackgroundBrush(AssociatedObject, new SolidColorBrush(foldingSelectBackgroundColor));
        FoldingMargin.SetFoldingMarkerBrush(AssociatedObject, new SolidColorBrush(foldingBackgroundColor));
        FoldingMargin.SetSelectedFoldingMarkerBrush(AssociatedObject, new SolidColorBrush(foldingSelectMarkerBackgroundColor));
    }

    private static Color GetColorFromResource(string resourceKey)
    {
        return (Color)ColorConverter.ConvertFromString(Application.Current.Resources[resourceKey]?.ToString());
    }
}