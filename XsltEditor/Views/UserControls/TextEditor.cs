using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Search;

using System.Windows;
using System.Windows.Input;

namespace XsltEditor.Views.UserControls;

public class TextEditor : ICSharpCode.AvalonEdit.TextEditor
{
    private FoldingManager? _foldingManager;

    public TextEditor()
    {
        Install();

        CommandBindings.Add(new CommandBinding(TextEditorCommands.ExpandAllFolds, ExpandAllFolds, CanExecuteFoldsCommand));
        CommandBindings.Add(new CommandBinding(TextEditorCommands.CollapseAllFolds, CollapseAllFolds, CanExecuteFoldsCommand));

        Loaded += TextEditor_Loaded;
        PreviewMouseWheel += TextEditor_PreviewMouseWheel;

        TextArea.Caret.PositionChanged += Caret_PositionChanged;
    }

    public static readonly DependencyProperty LineProperty =
        DependencyProperty.Register(nameof(Line), typeof(int), typeof(TextEditor), new PropertyMetadata(1, LineChanged));

    public static readonly DependencyProperty ColumnProperty =
        DependencyProperty.Register(nameof(Column), typeof(int), typeof(TextEditor), new PropertyMetadata(1, ColumnChanged));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextEditor), new PropertyMetadata(string.Empty, OnTextChanged));

    public int Line
    {
        get => (int)GetValue(LineProperty);
        set => SetValue(LineProperty, value);
    }

    public int Column
    {
        get => (int)GetValue(ColumnProperty);
        set => SetValue(ColumnProperty, value);
    }

    public new string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    internal string BaseText
    {
        get => base.Text;
        set => base.Text = value;
    }

    private void Install()
    {
        var strategy = new XmlFoldingStrategy();
        _foldingManager = FoldingManager.Install(TextArea);

        TextChanged += (_, _) =>
        {
            if (Document is null)
            {
                return;
            }

            strategy.UpdateFoldings(_foldingManager, Document);
        };

        SearchPanel.Install(TextArea);
    }

    private void TextEditor_Loaded(object sender, RoutedEventArgs e)
    {
        if (TextArea.LeftMargins.Count > 0 && TextArea.LeftMargins[1] is { } line)
        {
            line.Opacity = 0;
        }
    }

    private void Caret_PositionChanged(object? sender, EventArgs e)
    {
        if (sender is not Caret caret)
        {
            return;
        }

        Line = caret.Line;
        Column = caret.Column;
    }

    private void TextEditor_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            var fontSize = FontSize + e.Delta / 25.0;

            if (fontSize < 6)
            {
                FontSize = 6;
            }
            else
            {
                FontSize = fontSize > 200 ? 200 : fontSize;
            }

            e.Handled = true;
        }
    }

    private static void LineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var editor = (TextEditor)d;

        if (editor.IsLoaded)
        {
            editor.TextArea.Caret.Line = (int)e.NewValue;
            editor.ScrollToLine(editor.Line);
        }
    }

    private static void ColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var editor = (TextEditor)d;

        if (editor.IsLoaded)
        {
            editor.TextArea.Caret.Column = (int)e.NewValue;
        }
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var editor = (TextEditor)d;

        if (editor.BaseText != (string)e.NewValue)
        {
            editor.BaseText = (string)e.NewValue;
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        SetCurrentValue(TextProperty, BaseText);
        base.OnTextChanged(e);
    }

    private static void CollapseAllFolds(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is not TextEditor editor)
        {
            return;
        }

        editor.CollapseAllFolds();
    }

    private static void ExpandAllFolds(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is not TextEditor editor)
        {
            return;
        }

        editor.ExpandAllFolds();
    }

    private static void CanExecuteFoldsCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = false;
        e.Handled = true;

        if (sender is not TextEditor editor || editor._foldingManager?.AllFoldings == null)
        {
            return;
        }

        e.CanExecute = true;
    }

    private void CollapseAllFolds()
    {
        if (_foldingManager?.AllFoldings == null)
        {
            return;
        }

        foreach (var folding in _foldingManager.AllFoldings)
        {
            folding.IsFolded = true;
        }

        var firstFolding = _foldingManager.GetNextFolding(0);
        if (firstFolding != null)
        {
            firstFolding.IsFolded = false;
        }
    }

    private void ExpandAllFolds()
    {
        if (_foldingManager?.AllFoldings == null)
        {
            return;
        }

        foreach (var folding in _foldingManager.AllFoldings)
        {
            folding.IsFolded = false;
        }
    }
}

public static class TextEditorCommands
{
    public static readonly RoutedCommand ExpandAllFolds = new("ExpandAllFolds", typeof(TextEditor));
    public static readonly RoutedCommand CollapseAllFolds = new("CollapseAllFolds", typeof(TextEditor));
}