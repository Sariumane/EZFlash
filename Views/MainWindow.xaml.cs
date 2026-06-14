using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace EZFlash.Views;

public partial class MainWindow : Window
{
    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(
        IntPtr hwnd,
        int attr,
        ref int attrValue,
        int attrSize);

    private const int DWMWA_CAPTION_COLOR = 35;
    private const int DWMWA_TEXT_COLOR = 36;

    public MainWindow()
    {
        InitializeComponent();

        SourceInitialized += (_, _) =>
        {
            var hwnd = new WindowInteropHelper(this).Handle;

            int captionColor = ColorToAbgr(0x09, 0x31, 0x56); // #093156
            int textColor = ColorToAbgr(0xFF, 0xFF, 0xFF);    // White

            DwmSetWindowAttribute(hwnd, DWMWA_CAPTION_COLOR, ref captionColor, sizeof(int));
            DwmSetWindowAttribute(hwnd, DWMWA_TEXT_COLOR, ref textColor, sizeof(int));
        };
    }

    private static int ColorToAbgr(byte r, byte g, byte b)
    {
        return b << 16 | g << 8 | r;
    }
}