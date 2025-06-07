using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Search;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace XsltEditor.Views.UserControls;

[SupportedOSPlatform("windows")]
public class TextEditor : ICSharpCode.AvalonEdit.TextEditor
{
    public static readonly DependencyProperty LineProperty =
        DependencyProperty.Register(nameof(Line), typeof(int), typeof(TextEditor), new PropertyMetadata(1, LineChanged));

    public static readonly DependencyProperty ColumnProperty =
        DependencyProperty.Register(nameof(Column), typeof(int), typeof(TextEditor), new PropertyMetadata(1, ColumnChanged));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextEditor), new PropertyMetadata(string.Empty, OnTextChanged));

    public static readonly DependencyProperty CompletionDataProperty =
        DependencyProperty.Register(nameof(CompletionData), typeof(IList<CompletionData>), typeof(TextEditor), new PropertyMetadata(OnCompletionDataChanged));

    private readonly KeyEventHandler _previewKeyDownHandler;
    private readonly TextCompositionEventHandler _textEnteredHandler;

    private FoldingManager? _foldingManager;
    private CompletionWindow? _completionWindow;

    public TextEditor()
    {
        Install();

        CommandBindings.Add(new CommandBinding(TextEditorCommands.ExpandAllFolds, ExpandAllFolds, CanExecuteFoldsCommand));
        CommandBindings.Add(new CommandBinding(TextEditorCommands.CollapseAllFolds, CollapseAllFolds, CanExecuteFoldsCommand));

        Loaded += TextEditor_Loaded;
        PreviewMouseWheel += TextEditor_PreviewMouseWheel;
        TextArea.Caret.PositionChanged += Caret_PositionChanged;

        _textEnteredHandler = OnTextEntered;
        _previewKeyDownHandler = OnPreviewKeyDown;
    }

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
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private string BaseText
    {
        get => base.Text;
        set => base.Text = value;
    }

    public IList<CompletionData> CompletionData
    {
        get => (IList<CompletionData>)GetValue(CompletionDataProperty);
        set => SetValue(CompletionDataProperty, value);
    }

    private void Install()
    {
        SearchPanel.Install(TextArea);
        _foldingManager = FoldingManager.Install(TextArea);

        var strategy = new XmlFoldingStrategy();

        TextChanged += (_, _) =>
        {
            if (Document is null)
            {
                return;
            }

            strategy.UpdateFoldings(_foldingManager, Document);
        };
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
        if (Keyboard.Modifiers != ModifierKeys.Control)
        {
            return;
        }

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

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (_completionWindow == null || e.Key != Key.Enter)
        {
            return;
        }

        _completionWindow.CompletionList.RequestInsertion(e);
        e.Handled = true;
    }

    private void OnTextEntered(object sender, TextCompositionEventArgs e)
    {
        if (e.Text != "<")
        {
            return;
        }

        _completionWindow = new CompletionWindow(TextArea)
        {
            ResizeMode = ResizeMode.NoResize
        };

        var data = _completionWindow.CompletionList.CompletionData;

        foreach (var item in CompletionData)
        {
            data.Add(item);
        }

        _completionWindow.Closed += (_, _) => _completionWindow = null;
        _completionWindow.Show();
    }

    private static void LineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextEditor { IsLoaded: true } editor)
        {
            return;
        }

        int newLine = (int)e.NewValue;
        if (editor.TextArea.Caret.Line == newLine)
        {
            return;
        }

        editor.ScrollTo(newLine, editor.Column);
        editor.TextArea.Caret.Line = newLine;
    }

    private static void ColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextEditor { IsLoaded: true } editor)
        {
            editor.TextArea.Caret.Column = (int)e.NewValue;
        }
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextEditor editor && editor.BaseText != (string)e.NewValue)
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
        if (sender is TextEditor editor)
        {
            editor.CollapseAllFolds();
        }
    }

    private static void ExpandAllFolds(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is TextEditor editor)
        {
            editor.ExpandAllFolds();
        }
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

    private static void OnCompletionDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextEditor editor)
        {
            return;
        }

        editor.TextArea.TextEntered -= editor._textEnteredHandler;
        editor.PreviewKeyDown -= editor._previewKeyDownHandler;

        editor.TextArea.TextEntered += editor._textEnteredHandler;
        editor.PreviewKeyDown += editor._previewKeyDownHandler;
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

[SupportedOSPlatform("windows")]
public class CompletionData : ICompletionData
{
    public CompletionData(string text)
    {
        Text = text;
    }

    [JsonIgnore]
    public ImageSource? Image => null;

    public string Text { get; }

    [JsonIgnore]
    public object Content => Text;

    [JsonIgnore]
    public object Description => $"Insert {Text}";

    [JsonIgnore]
    public double Priority => 0;

    [JsonIgnore]
    public string OpenTag => $"<{Text}>";

    [JsonIgnore]
    public string CloseTag => $"</{Text}>";

    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
    {
        textArea.Document.Replace(completionSegment.Offset - 1, 1, string.Empty);
        textArea.Document.Replace(completionSegment, $"{OpenTag}{CloseTag}");

        textArea.Caret.Offset -= CloseTag.Length;
    }
}


public static class TextEditorCommands
{
    public static readonly RoutedCommand ExpandAllFolds = new("ExpandAllFolds", typeof(TextEditor));
    public static readonly RoutedCommand CollapseAllFolds = new("CollapseAllFolds", typeof(TextEditor));
}
