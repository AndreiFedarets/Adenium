using Adenium.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
