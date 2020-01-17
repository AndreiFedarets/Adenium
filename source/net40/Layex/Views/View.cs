using Layex.Controls;
using Layex.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Layex.Views
{
    public class View : UserControl
    {
        public static readonly DependencyProperty DisplayModeProperty;

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
            DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(DisplayMode), typeof(View), new PropertyMetadata(DisplayMode.Content, OnDisplayModePropertyChanged));
        }

        public View()
        {
            DataContextChanged += OnDataContextChanged;
        }

        public DisplayMode DisplayMode
        {
            get { return (DisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        private void RenderContent()
        {
            if (DisplayMode == DisplayMode.Content)
            {
                return;
            }
            ContentControl contentControl = ViewManager.FindViewContent(this);
            switch (DisplayMode)
            {
                case DisplayMode.Tab:
                    contentControl.Content = new ViewTabControl();
                    break;
                case DisplayMode.Tile:
                    contentControl.Content = new ViewTileControl();
                    break;
                case DisplayMode.Stack:
                    contentControl.Content = new ViewStackControl();
                    break;
                default:
                    throw new NotSupportedException($"DisplayMode.'{DisplayMode}' value is not supported");
            }
        }

        private static void OnDisplayModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            View view = (View)sender;
            view.RenderContent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IViewModel oldViewModel = e.OldValue as IViewModel;
            if (oldViewModel != null)
            {
                oldViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
            IViewModel newViewModel = e.NewValue as IViewModel;
            if (newViewModel != null)
            {
                newViewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
            RenderContent();
            UpdateAvailability();
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, nameof(IViewModel.Available), StringComparison.Ordinal))
            {
                UpdateAvailability();
            }
        }

        private void UpdateAvailability()
        {
            IViewModel viewModel = DataContext as IViewModel;
            if (viewModel == null)
            {
                return;
            }
            if (viewModel.Available)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }
    }
}
