using Adenium.ViewModels;
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
            ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IViewModel), typeof (ViewItem), new FrameworkPropertyMetadata(null));
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
