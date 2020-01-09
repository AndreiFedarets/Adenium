using System.Windows;
using System.Windows.Controls;

namespace Layex.Controls
{
    public class ViewStackControl : ItemsControl
    {
        static ViewStackControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewStackControl), new FrameworkPropertyMetadata(typeof(ViewStackControl)));
        }
    }
}
