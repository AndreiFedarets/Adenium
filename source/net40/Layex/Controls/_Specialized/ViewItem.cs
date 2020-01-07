using Layex.Extensions;
using Layex.Views;
using System.Windows;
using System.Windows.Controls;

namespace Layex.Controls
{
    public class ViewItem : ContentControl
    {
        public static readonly DependencyProperty DisplayPanelProperty;

        static ViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ViewItem), new FrameworkPropertyMetadata(typeof (ViewItem)));
            DisplayPanelProperty = DependencyProperty.Register("DisplayPanel", typeof(bool), typeof(ViewItem), new PropertyMetadata());
        }

        public bool DisplayPanel
        {
            get { return (bool)GetValue(DisplayPanelProperty); }
            set { SetValue(DisplayPanelProperty, value); }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            View parentView = this.FindParent<View>();
            if (parentView != null)
            {
                DisplayPanel = parentView.DisplayMode == DisplayMode.Grid;
            }
        }
    }
}
