using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Search;

using System.Windows;
using System.Windows.Input;

namespace XsltEditor.Views.UserControls;

public class TextEditor : ICSharpCode.AvalonEdit.TextEditor
{
    public TextEditor()
    {
        Install();

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
        var foldingManager = FoldingManager.Install(TextArea);

        TextChanged += (_, _) =>
        {
            if (Document is null)
            {
                return;
            }

            strategy.UpdateFoldings(foldingManager, Document);
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
}