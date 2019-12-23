using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class ViewGridControl : ItemsControl
    {
        static ViewGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewGridControl), new FrameworkPropertyMetadata(typeof(ViewGridControl)));
        }
    }
}
