using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class ViewTabControl : TabControl
    {
        static ViewTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewTabControl), new FrameworkPropertyMetadata(typeof(ViewTabControl)));
        }
    }
}
