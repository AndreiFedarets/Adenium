using Adenium.Layouts;
using Adenium.ViewModels;
using Adenium.Views;
using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class ViewItem : ContentControl
    {
        public static readonly DependencyProperty ViewModelProperty;
        public static readonly DependencyProperty DisplayPanelProperty;

        static ViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ViewItem), new FrameworkPropertyMetadata(typeof (ViewItem)));
            ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(ViewItem), new PropertyMetadata(OnViewModelPropertyChanged));
            DisplayPanelProperty = DependencyProperty.Register("DisplayPanel", typeof(bool), typeof(ViewItem), new PropertyMetadata());
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public bool DisplayPanel
        {
            get { return (bool)GetValue(DisplayPanelProperty); }
            set { SetValue(DisplayPanelProperty, value); }
        }

        private static void OnViewModelPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            ViewItem viewItem = (ViewItem)sender;
            object view = null;
            if (viewItem.ViewModel != null)
            {
                view = ViewManager.LocateViewForViewModel(viewItem.ViewModel);
                IItemsViewModel parent = viewItem.ViewModel.Parent;
                viewItem.DisplayPanel = parent != null && parent.DisplayMode == DisplayMode.Grid;
            }
            viewItem.Content = view;
        }
    }
}
