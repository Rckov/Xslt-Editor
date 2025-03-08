using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Input;

using XsltEditor.Infrastructure;

namespace XsltEditor.Views.UserControls;

[SupportedOSPlatform("windows")]
public partial class WindowBar
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(WindowBar), new PropertyMetadata(null));

    public static readonly DependencyProperty ShowTitleProperty =
        DependencyProperty.Register(nameof(ShowTitle), typeof(bool), typeof(WindowBar), new PropertyMetadata(null));

    public static readonly DependencyProperty LeftContentProperty =
        DependencyProperty.Register(nameof(LeftContent), typeof(object), typeof(WindowBar), new PropertyMetadata(null));

    public static readonly DependencyProperty RightContentProperty =
        DependencyProperty.Register(nameof(RightContent), typeof(object), typeof(WindowBar), new PropertyMetadata(null));

    public static readonly DependencyProperty CanMaximizedProperty =
        DependencyProperty.Register(nameof(CanMaximized), typeof(bool), typeof(WindowBar), new PropertyMetadata(true, null));

    public static readonly DependencyProperty CanMinimizedProperty =
        DependencyProperty.Register(nameof(CanMinimized), typeof(bool), typeof(WindowBar), new PropertyMetadata(true, null));

    public WindowBar()
    {
        InitializeComponent();
        InitCommands();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public bool ShowTitle
    {
        get => (bool)GetValue(ShowTitleProperty);
        set => SetValue(ShowTitleProperty, value);
    }

    public object LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public object RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public bool CanMaximized
    {
        get => (bool)GetValue(CanMaximizedProperty);
        set => SetValue(CanMaximizedProperty, value);
    }

    public bool CanMinimized
    {
        get => (bool)GetValue(CanMinimizedProperty);
        set => SetValue(CanMinimizedProperty, value);
    }

    public ICommand? CloseCommand { get; private set; }
    public ICommand? MaximizeCommand { get; private set; }
    public ICommand? MinimizeCommand { get; private set; }

    private void InitCommands()
    {
        CloseCommand = new RelayCommand(CloseWindowExecuter);
        MaximizeCommand = new RelayCommand(MaximizeWindowExecuter);
        MinimizeCommand = new RelayCommand(MinimizeWindowExecuter);
    }

    private void MinimizeWindowExecuter(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        window.WindowState = WindowState.Minimized;
    }

    private void MaximizeWindowExecuter(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        window.WindowState = window.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void CloseWindowExecuter(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        window.Close();
    }
}