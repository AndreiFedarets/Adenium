using System.Windows;
using System.Windows.Controls;

namespace Layex
{
    public class WindowManager : Caliburn.Micro.WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = base.EnsureWindow(model, view, isDialog);
            window.SizeToContent = SizeToContent.Manual;
            CloneProperties(window, view);
            return window;
        }

        protected virtual void CloneProperties(Window window, object viewObject)
        {
            Control viewControl = viewObject as Control;
            if (viewControl == null)
            {
                return;
            }
            Thickness padding = (Thickness)viewControl.GetValue(Control.PaddingProperty);
            Thickness margin = viewControl.Margin;
            double extraHeight = margin.Top + margin.Bottom + padding.Top + padding.Bottom + SystemParameters.WindowCaptionHeight + SystemParameters.ResizeFrameHorizontalBorderHeight * 2;
            double extraWidth = margin.Left + margin.Right + padding.Left + padding.Right + SystemParameters.ResizeFrameVerticalBorderWidth * 2;
            if (!double.IsNaN(viewControl.MinHeight) && !double.IsInfinity(viewControl.MinHeight))
            {
                window.MinHeight = viewControl.MinHeight + extraHeight;
                window.Height = window.MinHeight;
            }
            if (!double.IsNaN(viewControl.MinWidth) && !double.IsInfinity(viewControl.MinWidth))
            {
                window.MinWidth = viewControl.MinWidth + extraWidth;
                window.Width = window.MinWidth;
            }
            if (!double.IsNaN(viewControl.MaxHeight) && !double.IsInfinity(viewControl.MaxHeight))
            {
                window.MaxHeight = viewControl.MaxHeight = extraHeight;
            }
            if (!double.IsNaN(viewControl.MaxWidth) && !double.IsInfinity(viewControl.MaxWidth))
            {
                window.MaxWidth = viewControl.MaxWidth + extraWidth;
            }
            if (!double.IsNaN(viewControl.Height) && !double.IsInfinity(viewControl.Height))
            {
                window.Height = viewControl.Height + extraHeight;
                viewControl.Height = double.NaN;
            }
            if (!double.IsNaN(viewControl.Width) && !double.IsInfinity(viewControl.Width))
            {
                window.Width = viewControl.Width + extraWidth;
                viewControl.Width = double.NaN;
            }
            Views.View view = viewObject as Views.View;
            if (view != null)
            {
                window.WindowStyle = view.WindowStyle;
                window.WindowState = view.WindowState;
            }
        }
    }
}
