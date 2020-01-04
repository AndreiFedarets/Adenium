﻿using Layex.Controls;
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
            RenderContent();
        }
    }
}