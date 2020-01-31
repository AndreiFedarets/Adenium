using Layex.ViewModels;
using Layex.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Layex.Controls
{
    public class ViewItem : Control
    {
        public static readonly DependencyProperty DisplayPanelProperty;
        public static readonly DependencyProperty HeaderBackgroundProperty;
        public static readonly DependencyProperty ViewProperty;
        private static readonly DependencyPropertyKey ViewPropertyKey;

        static ViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ViewItem), new FrameworkPropertyMetadata(typeof (ViewItem)));
            DisplayPanelProperty = DependencyProperty.Register("DisplayPanel", typeof(bool), typeof(ViewItem), new PropertyMetadata());
            HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(ViewItem), new PropertyMetadata(Brushes.Transparent));
            ViewPropertyKey = DependencyProperty.RegisterReadOnly("View", typeof(UIElement), typeof(ViewItem), new PropertyMetadata(null));
            ViewProperty = ViewPropertyKey.DependencyProperty;
        }

        public ViewItem()
        {
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IViewModel viewModel = DataContext as IViewModel;
            if (viewModel != null)
            {
                View = ViewManager.LocateViewForViewModel(viewModel);
            }
            View view = View as View;
            if (view != null && view.DisplayPanel.HasValue)
            {
                DisplayPanel = view.DisplayPanel.Value;
            }
        }

        public bool DisplayPanel
        {
            get { return (bool)GetValue(DisplayPanelProperty); }
            set { SetValue(DisplayPanelProperty, value); }
        }

        public UIElement View
        {
            get { return (UIElement)GetValue(ViewProperty); }
            private set { SetValue(ViewPropertyKey, value); }
        }

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
    }
}
