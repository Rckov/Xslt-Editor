using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XsltEditor.Views.UserControls;

public partial class WindowBar : UserControl
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
    private void MinimizeWindow(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        window.WindowState = WindowState.Minimized;
    }

    [RelayCommand]
    private void MaximizeWindow(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        window.WindowState = window.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    [RelayCommand]
    private void CloseWindow(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        window.Close();
    }
}
