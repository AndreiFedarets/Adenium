using System;
using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class AutoPanel : Panel
    {
        static AutoPanel()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoPanel), new FrameworkPropertyMetadata(typeof(AutoPanel)));
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double y = 0;

            foreach (UIElement element in InternalChildren)
            {
                element.Arrange(new Rect(new Point(x, y), element.DesiredSize));
                x += 100;
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            availableSize = new Size(80, 80);
            foreach (UIElement element in InternalChildren)
            {
                if (element != null)
                {
                    element.Measure(availableSize);
                }
            }
            return default(Size);
        }
    }
}