using Adenium.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Adenium.Controls
{
    public class ViewModelToViewItemConverter : IValueConverter
    {
        private readonly IEnumerable<ViewItem> _viewItems;

        public ViewModelToViewItemConverter(IEnumerable<ViewItem> viewItems)
        {
            _viewItems = viewItems;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IViewModel viewModel = (IViewModel)values[0];
            IEnumerable<ViewItem> viewItems = (IEnumerable<ViewItem>)values[1];
            return viewItems.FirstOrDefault(x => ReferenceEquals(x.ViewModel, viewModel));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IViewModel viewModel = (IViewModel)value;
            ViewItem viewItem = _viewItems.FirstOrDefault(x => ReferenceEquals(x.ViewModel, viewModel));
            return viewItem;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ViewItem viewItem = (ViewItem)value;
            return viewItem.ViewModel;
        }
    }
}
