using Adenium.ViewModels;
using Adenium.Views;
using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class ViewItem : ContentControl
    {
        public static readonly DependencyProperty ViewModelProperty;

        static ViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ViewItem), new FrameworkPropertyMetadata(typeof (ViewItem)));
            ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(ViewItem), new PropertyMetadata(OnViewModelPropertyChanged));
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static void OnViewModelPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            ViewItem viewItem = (ViewItem)sender;
            object view = null;
            if (viewItem.ViewModel != null)
            {
                view = ViewManager.LocateViewForViewModel(viewItem.ViewModel);
            }
            viewItem.Content = view;
        }
    }
}
