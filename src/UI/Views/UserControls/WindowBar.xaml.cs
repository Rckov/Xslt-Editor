using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace XsltEditor.Views.UserControls;

public partial class WindowBar
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(WindowBar),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ShowTitleProperty =
        DependencyProperty.Register(nameof(ShowTitle), typeof(bool), typeof(WindowBar), new PropertyMetadata(false));

    public static readonly DependencyProperty LeftContentProperty =
        DependencyProperty.Register(nameof(LeftContent), typeof(object), typeof(WindowBar), new PropertyMetadata(null));

    public static readonly DependencyProperty RightContentProperty =
        DependencyProperty.Register(nameof(RightContent), typeof(object), typeof(WindowBar),
            new PropertyMetadata(null));

    public static readonly DependencyProperty CanMaximizedProperty =
        DependencyProperty.Register(nameof(CanMaximized), typeof(bool), typeof(WindowBar), new PropertyMetadata(true));

    public static readonly DependencyProperty CanMinimizedProperty =
        DependencyProperty.Register(nameof(CanMinimized), typeof(bool), typeof(WindowBar), new PropertyMetadata(true));

    public WindowBar()
    {
        InitializeComponent();
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

    [RelayCommand]
    private void MinimizeWindow(Window? parameter)
    {
        if (parameter is null)
        {
            return;
        }

        parameter.WindowState = WindowState.Minimized;
    }

    [RelayCommand]
    private void MaximizeWindow(Window? parameter)
    {
        if (parameter is null)
        {
            return;
        }

        parameter.WindowState = parameter.WindowState is WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    [RelayCommand]
    private void CloseWindow(Window? parameter)
    {
        parameter?.Close();
    }
}