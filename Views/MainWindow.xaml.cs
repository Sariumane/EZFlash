using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using EZFlash.ViewModels;

namespace EZFlash.Views;

public partial class MainWindow : Window
{

    // Import the DwmSetWindowAttribute function from dwmapi.dll
    [DllImport("dwmapi.dll")]

    // This function allows us to set various attributes of the window, including caption and text colors
    private static extern int DwmSetWindowAttribute(
        IntPtr hwnd,
        int attr,
        ref int attrValue,
        int attrSize);

    // Constants for the attributes we want to set (caption color and text color)
    private const int DWMWA_CAPTION_COLOR = 35;
    private const int DWMWA_TEXT_COLOR = 36;






    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel();

        //Set the caption and text colors when the window is initialized
        SourceInitialized += (_, _) =>
        {
            // Get the window handle (HWND) for the current window
            var hwnd = new WindowInteropHelper(this).Handle;

            int captionColor = ColorToAbgr(0x09, 0x31, 0x56); // #093156
            int textColor = ColorToAbgr(0xFF, 0xFF, 0xFF);    // White

            DwmSetWindowAttribute(hwnd, DWMWA_CAPTION_COLOR, ref captionColor, sizeof(int));
            DwmSetWindowAttribute(hwnd, DWMWA_TEXT_COLOR, ref textColor, sizeof(int));
        };
    }



    //Dirty Functions
    private void LearnSR_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new LearnSR();
    }

    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new HomeView();
    }


    //Helper Method to convert RGB to ABGR format
    private static int ColorToAbgr(byte r, byte g, byte b)
    {
        return b << 16 | g << 8 | r;
    }
}