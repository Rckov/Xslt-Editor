using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Search;

using System.Windows;
using System.Windows.Input;

using XsltEditor.Models;

namespace XsltEditor.Views.UserControls;

internal class TextEditor : ICSharpCode.AvalonEdit.TextEditor
{
	private const double MinFontSize = 6;
	private const double MaxFontSize = 200;
	private const double ZoomStep = 25.0;

	public static readonly DependencyProperty LineProperty =
		DependencyProperty.Register(nameof(Line), typeof(int), typeof(TextEditor),
			new PropertyMetadata(1, OnLineChanged));

	public static readonly DependencyProperty ColumnProperty =
		DependencyProperty.Register(nameof(Column), typeof(int), typeof(TextEditor),
			new PropertyMetadata(1, OnColumnChanged));

	public static readonly DependencyProperty TextProperty =
		DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextEditor),
			new PropertyMetadata(string.Empty, OnTextChanged));

	public static readonly DependencyProperty CompletionDataProperty =
		DependencyProperty.Register(nameof(CompletionData), typeof(IReadOnlyList<SnippetData>), typeof(TextEditor),
			new PropertyMetadata(null, OnCompletionDataChanged));

	private readonly FoldingManager? _foldingManager;
	private readonly XmlFoldingStrategy _foldingStrategy = new();

	private CompletionWindow? _completionWindow;
	private bool _completionAttached;

	public TextEditor()
	{
		SearchPanel.Install(TextArea);
		_foldingManager = FoldingManager.Install(TextArea);

		CommandBindings.Add(new CommandBinding(TextEditorCommands.ExpandAllFolds, OnExpandAllFolds, OnCanExecuteFolds));
		CommandBindings.Add(new CommandBinding(TextEditorCommands.CollapseAllFolds, OnCollapseAllFolds, OnCanExecuteFolds));

		Loaded += OnLoaded;
		PreviewMouseWheel += OnZoom;
		TextArea.Caret.PositionChanged += OnCaretPositionChanged;
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

	public IReadOnlyList<SnippetData>? CompletionData
	{
		get => (IReadOnlyList<SnippetData>?)GetValue(CompletionDataProperty);
		set => SetValue(CompletionDataProperty, value);
	}

	private string BaseText
	{
		get => base.Text;
		set => base.Text = value;
	}

	protected override void OnTextChanged(EventArgs e)
	{
		SetCurrentValue(TextProperty, BaseText);
		base.OnTextChanged(e);

		if (Document is not null && _foldingManager is not null)
		{
			_foldingStrategy.UpdateFoldings(_foldingManager, Document);
		}
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (TextArea.LeftMargins.Count > 1)
		{
			TextArea.LeftMargins[1].Opacity = 0;
		}
	}

	private void OnCaretPositionChanged(object? sender, EventArgs e)
	{
		if (sender is not Caret caret)
		{
			return;
		}

		Line = caret.Line;
		Column = caret.Column;
	}

	private void OnZoom(object sender, MouseWheelEventArgs e)
	{
		if (Keyboard.Modifiers is not ModifierKeys.Control)
		{
			return;
		}

		FontSize = Math.Clamp(FontSize + e.Delta / ZoomStep, MinFontSize, MaxFontSize);
		e.Handled = true;
	}

	private void OnTextEntered(object sender, TextCompositionEventArgs e)
	{
		if (e.Text is not "<" || CompletionData is null)
		{
			return;
		}

		_completionWindow = new CompletionWindow(TextArea) { ResizeMode = ResizeMode.NoResize };

		foreach (SnippetData item in CompletionData)
		{
			_completionWindow.CompletionList.CompletionData.Add(item);
		}

		_completionWindow.Closed += (_, _) => _completionWindow = null;
		_completionWindow.Show();
	}

	private void OnCompletionKeyDown(object sender, KeyEventArgs e)
	{
		if (_completionWindow is null || e.Key is not Key.Enter)
		{
			return;
		}

		_completionWindow.CompletionList.RequestInsertion(e);
		e.Handled = true;
	}

	private static void OnLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is not TextEditor { IsLoaded: true } editor)
		{
			return;
		}

		var line = (int)e.NewValue;
		if (editor.TextArea.Caret.Line == line)
		{
			return;
		}

		editor.ScrollTo(line, editor.Column);
		editor.TextArea.Caret.Line = line;
	}

	private static void OnColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

	private static void OnCompletionDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is not TextEditor editor)
		{
			return;
		}

		if (editor._completionAttached)
		{
			editor.TextArea.TextEntered -= editor.OnTextEntered;
			editor.PreviewKeyDown -= editor.OnCompletionKeyDown;
		}

		editor._completionAttached = e.NewValue is not null;

		if (editor._completionAttached)
		{
			editor.TextArea.TextEntered += editor.OnTextEntered;
			editor.PreviewKeyDown += editor.OnCompletionKeyDown;
		}
	}

	private void OnExpandAllFolds(object sender, ExecutedRoutedEventArgs e)
	{
		SetAllFoldings(false);
	}

	private void OnCollapseAllFolds(object sender, ExecutedRoutedEventArgs e)
	{
		SetAllFoldings(true);
		_foldingManager?.GetNextFolding(0)?.IsFolded = false;
	}

	private void OnCanExecuteFolds(object sender, CanExecuteRoutedEventArgs e)
	{
		e.CanExecute = _foldingManager?.AllFoldings is not null;
		e.Handled = true;
	}

	private void SetAllFoldings(bool folded)
	{
		if (_foldingManager?.AllFoldings is null)
		{
			return;
		}

		foreach (FoldingSection? folding in _foldingManager.AllFoldings)
		{
			folding.IsFolded = folded;
		}
	}
}

internal static class TextEditorCommands
{
	public static readonly RoutedCommand ExpandAllFolds = new(nameof(ExpandAllFolds), typeof(TextEditor));
	public static readonly RoutedCommand CollapseAllFolds = new(nameof(CollapseAllFolds), typeof(TextEditor));
}