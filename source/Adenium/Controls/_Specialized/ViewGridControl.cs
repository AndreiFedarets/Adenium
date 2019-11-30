using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class ViewGridControl : Grid
    {
        static ViewGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewGridControl), new FrameworkPropertyMetadata(typeof(ViewGridControl)));
        }

    }
}
