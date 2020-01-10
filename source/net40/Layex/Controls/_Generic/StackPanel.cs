using System.Windows;

namespace Layex.Controls
{
    public class StackPanel : System.Windows.Controls.StackPanel
    {
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    Size originalSize = base.MeasureOverride(constraint);
        //    Size scaledSize = StretchMeasuredSize(originalSize, constraint);
        //    if (originalSize != scaledSize)
        //    {
        //        ScaleElements(originalSize, scaledSize);
        //    }
        //    return scaledSize;
        //}

        //private void ScaleElements(Size originalSize, Size scaledSize)
        //{
        //    double scaleWidth = scaledSize.Width / originalSize.Width;
        //    double scaleHeight = scaledSize.Height / originalSize.Height;
        //    foreach (UIElement element in InternalChildren)
        //    {
        //        double desiredWidth = element.DesiredSize.Width * scaleWidth;
        //        double desiredHeigth = element.DesiredSize.Height * scaleHeight;
        //        Size desiredSize = new Size(desiredWidth, desiredHeigth);
        //        element.Measure(desiredSize);
        //    }
        //}

        //private Size StretchMeasuredSize(Size measuredSize, Size constraint)
        //{
        //    if (Orientation == System.Windows.Controls.Orientation.Horizontal)
        //    {
        //        if (HorizontalAlignment == HorizontalAlignment.Stretch && !double.IsPositiveInfinity(constraint.Width))
        //        {
        //            measuredSize.Width = constraint.Width;
        //        }
        //    }
        //    else if (Orientation == System.Windows.Controls.Orientation.Vertical)
        //    {
        //        if (VerticalAlignment == VerticalAlignment.Stretch && !double.IsPositiveInfinity(constraint.Height))
        //        {
        //            measuredSize.Height = constraint.Height;
        //        }
        //    }
        //    return measuredSize;
        //}
    }
}
