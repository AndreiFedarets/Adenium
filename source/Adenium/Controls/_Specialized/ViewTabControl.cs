using Adenium.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Adenium.Controls
{
    public class ViewTabControl : ViewItemsControl
    {
        public static readonly DependencyProperty ViewItemConverterProperty;

        static ViewTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ViewItemsControl), new FrameworkPropertyMetadata(typeof (ViewItemsControl)));
            ViewItemConverterProperty = DependencyProperty.Register("ViewItemConverter", typeof(IValueConverter), typeof(ViewTabControl), new PropertyMetadata(null));
        }

        public ViewTabControl()
        {
            ViewItemConverter = new ViewModelToViewItemConverter(Items);
        }

        public IValueConverter ViewItemConverter
        {
            get { return (IValueConverter)GetValue(ViewItemConverterProperty); }
            set { SetValue(ViewItemConverterProperty, value); }
        }

        private void OnActiveItemChanged(ViewItem newValue, ViewItem oldValue)
        {
            IViewModel activeViewModel = null;
            if (!Items.Contains(newValue))
            {
                newValue = oldValue;
                return;
            }
            ViewModel.ActiveItem = activeViewModel;
        }


        //TabControl tabControl = new TabControl()
        //{
        //    HorizontalAlignment = HorizontalAlignment.Stretch,
        //    VerticalAlignment = VerticalAlignment.Stretch,
        //    ItemTemplate = (DataTemplate)Application.Current.Resources["TabViewHeaderDataTempate"],
        //    //ContentTemplate = (DataTemplate)Application.Current.Resources["TabViewContentDataTempate"],
        //    DataContext = this,
        //    ItemsSource = Items
        //};

        ////Setup binding this.ActiveItem <-> tabControl.SelectedItem
        //Binding selectedItemBinding = new Binding("ActiveItem");
        //selectedItemBinding.Mode = BindingMode.TwoWay;
        //    tabControl.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);
    }
}
