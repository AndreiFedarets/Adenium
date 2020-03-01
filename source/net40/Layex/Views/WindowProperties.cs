using System.Windows;

namespace Layex.Views
{
    public static class WindowProperties
    {
        public static readonly DependencyProperty WindowStyleProperty;
        public static readonly DependencyProperty WindowStateProperty;
        public static readonly DependencyProperty ResizeModeProperty;

        static WindowProperties()
        {
            WindowStyleProperty = DependencyProperty.RegisterAttached("WindowStyle", typeof(WindowStyle), typeof(WindowProperties), new PropertyMetadata(WindowStyle.SingleBorderWindow, OnWindowStylePropertyChanged));
            WindowStateProperty = DependencyProperty.RegisterAttached("WindowState", typeof(WindowState), typeof(WindowProperties), new PropertyMetadata(WindowState.Normal, OnWindowStatePropertyChanged));
            ResizeModeProperty = DependencyProperty.RegisterAttached("ResizeMode", typeof(ResizeMode), typeof(WindowProperties), new PropertyMetadata(ResizeMode.CanResize, OnResizeModePropertyChanged));
        }

        public static void SetWindowStyle(DependencyObject element, WindowStyle value)
        {
            element.SetValue(WindowStyleProperty, value);
        }

        public static WindowStyle GetWindowStyle(DependencyObject element)
        {
            return (WindowStyle)element.GetValue(WindowStyleProperty);
        }

        public static void SetWindowState(DependencyObject element, WindowState value)
        {
            element.SetValue(WindowStateProperty, value);
        }

        public static WindowState GetWindowState(DependencyObject element)
        {
            return (WindowState)element.GetValue(WindowStateProperty);
        }

        public static void SetResizeMode(DependencyObject element, ResizeMode value)
        {
            element.SetValue(ResizeModeProperty, value);
        }

        public static ResizeMode GetResizeMode(DependencyObject element)
        {
            return (ResizeMode)element.GetValue(ResizeModeProperty);
        }

        private static void OnWindowStylePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {

        }

        private static void OnWindowStatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {

        }

        private static void OnResizeModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {

        }

    }
}
