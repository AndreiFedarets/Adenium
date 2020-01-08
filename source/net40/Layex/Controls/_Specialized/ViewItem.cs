using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Layex.Controls
{
    public class ViewItem : ContentControl
    {
        public static readonly DependencyProperty DisplayPanelProperty;
        public static readonly DependencyProperty HeaderBackgroundProperty;

        static ViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ViewItem), new FrameworkPropertyMetadata(typeof (ViewItem)));
            DisplayPanelProperty = DependencyProperty.Register("DisplayPanel", typeof(bool), typeof(ViewItem), new PropertyMetadata());
            HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(ViewItem), new PropertyMetadata(Brushes.Transparent));
        }

        public bool DisplayPanel
        {
            get { return (bool)GetValue(DisplayPanelProperty); }
            set { SetValue(DisplayPanelProperty, value); }
        }

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
    }
}
