using Layex.Controls;
using Layex.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Layex.Views
{
    public class View : UserControl
    {
        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
        }

        public View()
        {
            DataContextChanged += OnDataContextChanged;
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
        }

        private void RenderContent()
        {
            ItemsViewModel itemsViewModel = ViewModel as ItemsViewModel;
            if (itemsViewModel != null)
            {
                ContentControl contentControl = ViewManager.FindViewContent(this);
                switch (itemsViewModel.DisplayMode)
                {
                    case Layouts.DisplayMode.Tab:
                        contentControl.Content = new ViewTabControl() { DataContext = itemsViewModel };
                        break;
                    case Layouts.DisplayMode.Grid:
                        contentControl.Content = new ViewGridControl() { DataContext = itemsViewModel };
                        break;
                    default:
                        throw new NotSupportedException($"DisplayMode.'{itemsViewModel.DisplayMode}' value is not supported");
                }
            }
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ItemsViewModel itemsViewModel = ViewModel as ItemsViewModel;
            if (itemsViewModel != null)
            {
                if (string.Equals(nameof(itemsViewModel.DisplayMode), e.PropertyName, StringComparison.Ordinal))
                {
                    RenderContent();
                }
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IViewModel previousViewModel = e.OldValue as IViewModel;
            if (previousViewModel != null)
            {
                previousViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
            IViewModel newViewModel = e.NewValue as IViewModel;
            if (newViewModel != null)
            {
                newViewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
            RenderContent();
        }
    }
}
