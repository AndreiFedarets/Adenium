﻿using Layex.Controls;
using Layex.Extensions;
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
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            DataContextChanged += OnDataContextChanged;
        }

        public DisplayMode DisplayMode
        {
            get { return (DisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            View parentView = this.FindParent<View>();
            if (parentView == null || parentView.DisplayMode != DisplayMode.Grid)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch;
                VerticalAlignment = VerticalAlignment.Stretch;
            }
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
                    contentControl.Content = new ViewTabControl() { DataContext = DataContext };
                    break;
                case DisplayMode.Grid:
                    contentControl.Content = new ViewGridControl() { DataContext = DataContext };
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
