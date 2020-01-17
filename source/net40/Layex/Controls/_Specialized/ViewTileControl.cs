using System.Windows;
using System.Windows.Controls;

namespace Layex.Controls
{
    public class ViewTileControl : ItemsControl
    {
        static ViewTileControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewTileControl), new FrameworkPropertyMetadata(typeof(ViewTileControl)));
        }
    }
}
