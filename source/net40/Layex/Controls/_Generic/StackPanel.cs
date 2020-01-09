using System.Windows;

namespace Layex.Controls
{
    public class StackPanel : System.Windows.Controls.StackPanel
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Size size = base.MeasureOverride(constraint);
            if (HorizontalAlignment == HorizontalAlignment.Stretch && !double.IsPositiveInfinity(constraint.Width))
            {
                size.Width = constraint.Width;
            }
            if (VerticalAlignment == VerticalAlignment.Stretch && !double.IsPositiveInfinity(constraint.Height))
            {
                size.Height = constraint.Height;
            }
            return size;
        }
    }
}
