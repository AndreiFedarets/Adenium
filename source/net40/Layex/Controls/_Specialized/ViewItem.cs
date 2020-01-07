using Layex.Extensions;
using Layex.Views;
using System.Windows;
using System.Windows.Controls;

namespace Layex.Controls
{
    public class ViewItem : ContentControl
    {
        public static readonly DependencyProperty DisplayPanelProperty;
        private ContentControl _contentPresenter;

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

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            FrameworkElement newElement = newContent as FrameworkElement;
            if (newElement != null)
            {
                Height = newElement.Height;
                newElement.Height = double.NaN;
                Width = newElement.Width;
                newElement.Width = double.NaN;
            }
        }

        //public override void OnApplyTemplate()
        //{
        //    _contentPresenter = GetTemplateChild("ContentPresenter") as ContentControl;
        //    if (_contentPresenter != null)
        //    {
                
        //    }
        //    base.OnApplyTemplate();
        //}
    }
}
