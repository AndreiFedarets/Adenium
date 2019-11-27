using System.Windows;

namespace Adenium.Controls
{
    public class ViewGridControl : ViewItemsControl
    {
        static ViewGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ViewGridControl), new FrameworkPropertyMetadata(typeof (ViewGridControl)));
        }

    }
}
