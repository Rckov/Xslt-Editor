using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace XsltEditor.Controls;

public class Window : System.Windows.Window
{
    #region Enums

    private enum SizeEvents
    {
        SIZE_RESTORED = 0,
        SIZE_MAXIMIZED = 2
    }

    #endregion

    #region Constants

    private const int MONITOR_DEFAULTTONEAREST = 2;
    private const int WM_ENTERSIZEMOVE = 0x0231;
    private const int WM_EXITSIZEMOVE = 0x0232;
    private const int WM_SIZE = 0x0005;

    #endregion

    #region P/Invoke Declarations

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr MonitorFromWindow(IntPtr handle, int dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

    #endregion

    #region Methods

    private static void DisableBlur(IntPtr hWnd)
    {
        if (Environment.OSVersion.Version.Major < 10 || Environment.OSVersion.Version.Build < 10586)
        {
            return;
        }

        AcrylicHelper.SetBlur(hWnd, AccentState.ACCENT_DISABLED);

        var source = HwndSource.FromHwnd(hWnd);
        source?.RemoveHook(WndProc);
    }

    private static void EnableBlur(IntPtr hWnd)
    {
        if (Environment.OSVersion.Version.Major < 10 || Environment.OSVersion.Version.Build < 10586)
        {
            return;
        }

        AcrylicHelper.SetBlur(hWnd, AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND);

        var source = HwndSource.FromHwnd(hWnd);
        if (source != null)
        {
            var window = (Window)source.RootVisual;
            source.AddHook(WndProc);

            if (window.WindowState == WindowState.Maximized)
            {
                FixMaximizedWindowSize(hWnd);
            }

            window.ContentRendered += OnContentRendered;
        }

        InvalidateRect(hWnd, IntPtr.Zero, true);
    }

    private static void FixMaximizedWindowSize(IntPtr handle)
    {
        var monitor = MonitorFromWindow(handle, MONITOR_DEFAULTTONEAREST);
        var monitorInfo = new MONITORINFO
        {
            cbSize = Marshal.SizeOf<MONITORINFO>()
        };

        if (!GetMonitorInfo(monitor, ref monitorInfo))
        {
            return;
        }

        var workingRectangle = monitorInfo.rcWork;
        var width = Math.Abs(workingRectangle.right - workingRectangle.left);
        var height = Math.Abs(workingRectangle.bottom - workingRectangle.top);

        MoveWindow(handle, workingRectangle.left, workingRectangle.top, width, height, true);
    }

    private static void OnContentRendered(object? sender, EventArgs e)
    {
        if (sender is Window window && window.SizeToContent != SizeToContent.Manual)
        {
            window.InvalidateMeasure();
        }

        if (sender is Window wnd)
        {
            wnd.ContentRendered -= OnContentRendered;
        }
    }

    private static IntPtr WndProc(IntPtr handle, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case WM_ENTERSIZEMOVE:
                AcrylicHelper.SetBlur(handle, AccentState.ACCENT_ENABLE_BLURBEHIND);
                InvalidateRect(handle, IntPtr.Zero, true);
                break;

            case WM_EXITSIZEMOVE:
                AcrylicHelper.SetBlur(handle, AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND);
                InvalidateRect(handle, IntPtr.Zero, true);
                break;

            case WM_SIZE when wParam == (IntPtr)SizeEvents.SIZE_MAXIMIZED:
                FixMaximizedWindowSize(handle);
                break;
        }

        return IntPtr.Zero;
    }

    #endregion

    #region Attached Property

    public static readonly DependencyProperty AcrylicEffectProperty =
        DependencyProperty.Register(nameof(AcrylicEffect), typeof(bool), typeof(Window), new PropertyMetadata(false, OnAcrylicEffectChanged));

    public bool AcrylicEffect
    {
        get => (bool)GetValue(AcrylicEffectProperty);
        set => SetValue(AcrylicEffectProperty, value);
    }

    private static void OnAcrylicEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Window window)
        {
            return;
        }

        var helper = new WindowInteropHelper(window);

        if ((bool)e.NewValue)
        {
            window.AllowsTransparency = true;
            window.WindowStyle = WindowStyle.None;

            window.Loaded += (s, e) => EnableBlur(helper.Handle);

            if (window.IsLoaded)
            {
                EnableBlur(helper.Handle);
            }
        }
        else
        {
            window.AllowsTransparency = false;
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            DisableBlur(helper.Handle);
        }
    }

    #endregion

    #region Structs

    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public int dwFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    #endregion
}

#region AcrylicHelper Class

internal static class AcrylicHelper
{
    public static AccentState CurrentAccentState { get; private set; }

    public static void SetBlur(IntPtr handle, AccentState accentState, bool isWindow = true)
    {
        CurrentAccentState = accentState;

        var accent = new AccentPolicy
        {
            AccentState = accentState,
            AccentFlags = (uint)(isWindow ? 2 : 0x20 | 0x40 | 0x80 | 0x100),
            GradientColor = 0x00FFFFFF
        };

        var accentStructSize = Marshal.SizeOf(accent);
        var accentPtr = Marshal.AllocHGlobal(accentStructSize);

        try
        {
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            if (SetWindowCompositionAttribute(handle, ref data) != 0)
            {
                // Handle error if needed
            }
        }
        finally
        {
            Marshal.FreeHGlobal(accentPtr);
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowCompositionAttribute(IntPtr handle, ref WindowCompositionAttributeData data);

    internal enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public uint AccentFlags;
        public uint GradientColor;
        public uint AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
}

internal enum AccentState
{
    ACCENT_DISABLED = 0,
    ACCENT_ENABLE_GRADIENT = 1,
    ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
    ACCENT_ENABLE_BLURBEHIND = 3,
    ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
    ACCENT_INVALID_STATE = 5
}

#endregion