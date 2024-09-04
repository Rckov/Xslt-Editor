using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;

using Microsoft.Xaml.Behaviors;

using System.Windows;

namespace XsltEditor.Behaviors;

internal class TextEditorTextBehavior : Behavior<TextEditor>
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
       nameof(Text),
       typeof(string),
       typeof(TextEditorTextBehavior),
       new PropertyMetadata(string.Empty, OnTextChanged));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null)
        {
            return;
        }

        SearchPanel.Install(AssociatedObject);
        AssociatedObject.TextChanged += OnTextEditorTextChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is null)
        {
            return;
        }

        AssociatedObject.TextChanged -= OnTextEditorTextChanged;
    }

    private void OnTextEditorTextChanged(object? sender, EventArgs e)
    {
        if (AssociatedObject is null || AssociatedObject.Text == Text)
        {
            return;
        }

        Text = AssociatedObject.Text;
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (TextEditorTextBehavior)d;

        if (behavior.AssociatedObject is null)
        {
            return;
        }

        var newText = (string)e.NewValue;

        if (newText == behavior.AssociatedObject.Text)
        {
            return;
        }

        behavior.AssociatedObject.Text = newText;
    }
}