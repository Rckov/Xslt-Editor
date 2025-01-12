using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Input;

using XsltEditor.Tools.Commands;

namespace XsltEditor.Controls;

[SupportedOSPlatform("windows")]
public partial class WindowBar
{
    public static readonly DependencyProperty ShowTitleProperty =
        DependencyProperty.Register(nameof(ShowTitle), typeof(bool), typeof(WindowBar), new PropertyMetadata(null));

    public static readonly DependencyProperty CustomContentProperty =
        DependencyProperty.Register(nameof(CustomContent), typeof(object), typeof(WindowBar), new PropertyMetadata(null));

    public WindowBar()
    {
        InitializeComponent();
        InitCommands();
    }

    public bool ShowTitle
    {
        get => (bool)GetValue(ShowTitleProperty);
        set => SetValue(ShowTitleProperty, value);
    }

    public object CustomContent
    {
        get => GetValue(CustomContentProperty);
        set => SetValue(CustomContentProperty, value);
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